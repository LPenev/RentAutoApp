namespace RentAutoApp.Web.ViewModels.Search;

public class CarSearchViewModel
{
    public int? SelectedLocationId { get; set; }
    public int? SelectedCarTypeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public IEnumerable<IdNameDto> Locations { get; set; } = new HashSet<IdNameDto>();
    public IEnumerable<IdNameDto> CarTypes { get; set; } = new HashSet<IdNameDto>();
}

public class IdNameDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}


