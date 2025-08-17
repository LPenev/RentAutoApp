using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RentAutoApp.Data.Models;
using RentAutoApp.Web.Data;

namespace RentAutoApp.Services.Core.Tests.Services
{
    [TestFixture]
    public class SettingsServiceTests
    {
        private static RentAutoAppDbContext NewDb(string name)
        {
            var options = new DbContextOptionsBuilder<RentAutoAppDbContext>()
                .UseInMemoryDatabase(databaseName: name)
                .Options;
            return new RentAutoAppDbContext(options);
        }

        [Test]
        public async Task GetAsync_Returns_Null_When_Missing_And_Caches_Null()
        {
            using var db = NewDb(nameof(GetAsync_Returns_Null_When_Missing_And_Caches_Null));
            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new SettingsService(db, cache);

            var v1 = await sut.GetAsync("NoSuchKey");
            Assert.That(v1, Is.Null);

            // Insert in DB after first get; due to cached null, second get should still be null
            db.SiteSettings.Add(new SiteSetting { Key = "NoSuchKey", Value = "later" });
            await db.SaveChangesAsync();

            var v2 = await sut.GetAsync("NoSuchKey");
            Assert.That(v2, Is.Null, "Expected cached null value until cache invalidation");
        }

        [Test]
        public async Task GetAsync_Returns_Value_And_Uses_Cache()
        {
            using var db = NewDb(nameof(GetAsync_Returns_Value_And_Uses_Cache));
            db.SiteSettings.Add(new SiteSetting { Key = "Contact.RecipientEmail", Value = "first@example.com" });
            await db.SaveChangesAsync();

            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new SettingsService(db, cache);

            // First call populates cache
            var v1 = await sut.GetAsync("Contact.RecipientEmail");
            Assert.That(v1, Is.EqualTo("first@example.com"));

            // Change DB value; should still get cached first value
            var e = await db.SiteSettings.FirstAsync(s => s.Key == "Contact.RecipientEmail");
            e.Value = "changed@example.com";
            await db.SaveChangesAsync();

            var v2 = await sut.GetAsync("Contact.RecipientEmail");
            Assert.That(v2, Is.EqualTo("first@example.com"), "Should be served from cache");
        }

        [Test]
        public async Task SetAsync_Creates_Or_Updates_And_Invalidates_Cache()
        {
            using var db = NewDb(nameof(SetAsync_Creates_Or_Updates_And_Invalidates_Cache));
            db.SiteSettings.Add(new SiteSetting { Key = "Site.Title", Value = "OldTitle" });
            await db.SaveChangesAsync();

            using var cache = new MemoryCache(new MemoryCacheOptions());
            var sut = new SettingsService(db, cache);

            // Warm up cache with old value
            var oldVal = await sut.GetAsync("Site.Title");
            Assert.That(oldVal, Is.EqualTo("OldTitle"));

            // Update via service; it should remove cache entry
            await sut.SetAsync("Site.Title", "NewTitle");

            // Now GetAsync should hit DB (since cache was removed) and fetch new value, then cache it
            var newVal = await sut.GetAsync("Site.Title");
            Assert.That(newVal, Is.EqualTo("NewTitle"));

            // Create new key
            await sut.SetAsync("New.Key", "Value123");
            var created = await sut.GetAsync("New.Key");
            Assert.That(created, Is.EqualTo("Value123"));
        }
    }
}
