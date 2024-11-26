using System;

namespace KartApplication.Helpers
{
    public static class GuidHelper
    {
        public static string ShortenGuid(string guid)
        {
            // Kontroller om GUID er i et gyldig format
            if (!Guid.TryParse(guid, out Guid parsedGuid))
            {
                throw new ArgumentException("Ugyldig GUID-format.", nameof(guid));
            }

            // Konverter GUID til en byte-array
            var bytes = parsedGuid.ToByteArray();

            // Konverter byte-array til Base64-format
            string base64 = Convert.ToBase64String(bytes);

            // Rens den resulterende Base64-strengen (fjern /, +, = tegn)
            base64 = base64.Replace("/", "").Replace("+", "").Replace("=", "");

            // Ta de første 9 tegnene og returner resultatet
            return base64.Length > 9 ? base64.Substring(0, 9) : base64;
        }
    }
}
