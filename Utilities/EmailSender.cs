using Microsoft.AspNetCore.Identity.UI.Services;

namespace KartApplication.Utilities
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implementasjon for å sende en e-post skal legges til her
            // Foreløpig returnerer denne metoden bare en fullført oppgave

            return Task.CompletedTask;
        }
    }
}
