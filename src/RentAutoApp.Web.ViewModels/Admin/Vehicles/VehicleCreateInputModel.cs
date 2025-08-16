using RentAutoApp.GCommon.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentAutoApp.Web.ViewModels.Admin.Vehicles;

public class VehicleCreateInputModel
{
    [Required, StringLength(100)]
    public string Brand { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Model { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Vin { get; set; } = string.Empty;

    [Display(Name = "Първа регистрация"), DataType(DataType.Date)]
    public DateTime FirstRegistrationDate { get; set; } = DateTime.UtcNow.Date;

    [Display(Name = "Гориво"), Required]
    public FuelType FuelType { get; set; }

    [Display(Name = "Скоростна кутия"), Required]
    public TransmissionType Transmission { get; set; }

    [Range(20, 2000)]
    public int PowerHp { get; set; }

    [Range(0, 10000)]
    public decimal PricePerDay { get; set; }

    [Range(0, 1000)]
    public decimal PricePerHour { get; set; }

    public bool IsAvailable { get; set; } = true;

    [Display(Name = "Категория"), Required]
    public int SubCategoryId { get; set; }

    [Display(Name = "Локация"), Required]
    public int LocationId { get; set; }
}


