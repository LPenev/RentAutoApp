namespace RentAutoApp.GCommon;

public class Constants
{
    public class Vehicle
    { 
        public const string VehicleIsAvailable = "Налична";
        public const string VehicleNotAvailable = "Не е налична";
        public const string LabelBack = "Обратно";
        public const string NotAvailableForRent = "Не е налична за наем";
    }

    public class Contact
    {
        public const string LabelContactEmailSentTo = "Contact email sent to {Recipient}";
        public const string LabelNewRequestFromSite = "Ново запитване от сайта";
        public const string LabelContactOK = "Благодарим за съобщението! Ще се свържем с вас скоро.";
    }

    public const string CultureBulgarian = "bg";
    public const string CultureEnglish = "en";
    public const string CultureGerman = "de";

    public const string DefaultCulture = CultureEnglish;

    public static readonly string[] SupportedCultures = { CultureBulgarian, CultureEnglish, CultureGerman };
}

