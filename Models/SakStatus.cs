namespace KartApplication.Models
{
    public enum SakStatus
    {
        SakMottatt,         // "Sak alındı"
        UnderBehandling,    // "Sak işleniyor"
        Ferdigstilt,        // "Sak tamamlandı"
        Avsluttet           // "Sak kapatıldı"
    }

    public static class SakStatusHelper
    {
        public static string GetMessage(SakStatus status)
        {
            switch (status)
            {
                case SakStatus.SakMottatt:
                    return "Det er bekreftet at din sak er mottatt. Det vil bli tatt opp og tilbakemelding vil bli gitt til deg snart som mulig.";
                case SakStatus.UnderBehandling:
                    return "Saken din blir behandlet.";
                case SakStatus.Ferdigstilt:
                    return "Saken er ferdigstilt og nødvendige endringer er gjort.";
                case SakStatus.Avsluttet:
                    return "Saken er avsluttet. Ingen endringer ble gjort.";
                default:
                    return "Ukjent status.";
            }
        }
    }
}
