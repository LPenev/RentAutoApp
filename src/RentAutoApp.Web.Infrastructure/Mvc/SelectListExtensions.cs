using Microsoft.AspNetCore.Mvc.Rendering;
using RentAutoApp.Web.ViewModels.Reservations;

namespace RentAutoApp.Web.Infrastructure.Mvc
{
    public static class SelectListExtensions
    {
        // for LocationOptionViewModel List<LocationOptionViewModel> to IEnumerable<SelectListItem>
        public static IEnumerable<SelectListItem> ToSelectList(
            this IEnumerable<LocationOptionViewModel> source,
            int? selectedId = null)
            => source.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Text,
                Selected = selectedId.HasValue && x.Id == selectedId.Value
            });
    }
}
