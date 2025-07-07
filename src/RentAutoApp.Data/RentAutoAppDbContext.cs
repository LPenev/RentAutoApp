using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RentAutoApp.Web.Data;

public class RentAutoAppDbContext : IdentityDbContext
{
    public RentAutoAppDbContext(DbContextOptions<RentAutoAppDbContext> options)
        : base(options)
    {
    }
}