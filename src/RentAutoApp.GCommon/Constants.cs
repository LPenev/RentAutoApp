namespace RentAutoApp.GCommon;

public class Constants
{

    public class Contact
    {
        public const string LabelContactEmailSentTo = "Contact email sent to {Recipient}";
        public const string LabelNewRequestFromSite = "Ново запитване от сайта";
    }

    public const string CultureBulgarian = "bg";
    public const string CultureEnglish = "en";
    public const string CultureGerman = "de";

    public const string DefaultCulture = CultureEnglish;

    public static readonly string[] SupportedCultures = { CultureBulgarian, CultureEnglish, CultureGerman };
}

