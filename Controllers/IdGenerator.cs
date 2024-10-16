using System;

public class IdGenerator
{
    // GUID'i 8 haneli sayısal bir ID'ye dönüştüren fonksiyon
    public static string GenerateNumericIdFromGuid()
    {
        // Yeni bir GUID oluştur ve string olarak al
        string guid = Guid.NewGuid().ToString();

        // GUID'in hash kodunu al ve mutlak değere çevir
        long numericId = Math.Abs(guid.GetHashCode());

        // 8 haneli sayıya dönüştürmek için mod al
        long eightDigitId = numericId % 100000000; // 8 haneli bir sayı için mod alıyoruz

        // 8 haneli olarak döndür
        return eightDigitId.ToString("D8"); // 8 haneli olarak formatla
    }
}