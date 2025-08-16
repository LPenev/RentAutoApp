using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.Reservations;

public class ReservationCreateInputModel
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    [Display(Name = "Място за вземане")]
    public int PickupLocationId { get; set; }

    [Required]
    [Display(Name = "Място за връщане")]
    public int ReturnLocationId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Начална дата")]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Крайна дата")]
    public DateTime EndDate { get; set; }

    public string? VehicleTitle { get; set; }
    public decimal? PricePerDay { get; set; }

    public List<LocationOptionViewModel> Locations { get; set; } = new();
}


