using System;

namespace KartApplication.Helpers
{
    public static class GuidHelper
    {
        public static string ShortenGuid(string guid)
        {
            // GUID'yi geçerli bir formatta olup olmadığını kontrol edin
            if (!Guid.TryParse(guid, out Guid parsedGuid))
            {
                throw new ArgumentException("Geçersiz GUID formatı.", nameof(guid));
            }

            // GUID'yi byte dizisine çevir
            var bytes = parsedGuid.ToByteArray();

            // Byte dizisini Base64 formatına çevir
            string base64 = Convert.ToBase64String(bytes);

            // Base64 formatında elde edilen stringi temizle (/, +, = karakterlerini kaldır)
            base64 = base64.Replace("/", "").Replace("+", "").Replace("=", "");

            // Sonuç olarak ilk 9 karakteri al ve döndür
            return base64.Length > 9 ? base64.Substring(0, 9) : base64;
        }
    }
}