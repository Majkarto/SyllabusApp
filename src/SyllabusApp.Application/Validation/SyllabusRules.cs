namespace SyllabusApp.Application.Validation;

/// <summary>
/// Reguły poprawności liczby godzin w sylabusie.
/// Przyjęto, że 1 punkt ECTS odpowiada 25-30 godzinom pracy studenta.
/// </summary>
public static class SyllabusRules
{
    public const int MinEcts = 1;
    public const int MaxEcts = 30;
    public const int MinHoursPerEcts = 25;
    public const int MaxHoursPerEcts = 30;

    /// <summary>Sprawdza godziny zajęć z formularza sylabusa.</summary>
    public static List<string> ValidateHours(int ects, int lectureHours, int exerciseHours, int laboratoryHours)
    {
        var errors = new List<string>();

        if (ects < MinEcts || ects > MaxEcts)
            errors.Add($"Liczba punktów ECTS musi mieścić się w zakresie {MinEcts}-{MaxEcts}.");

        if (lectureHours < 0 || exerciseHours < 0 || laboratoryHours < 0)
            errors.Add("Liczba godzin nie może być ujemna.");

        var contactHours = lectureHours + exerciseHours + laboratoryHours;
        if (contactHours <= 0)
            errors.Add("Sylabus musi mieć co najmniej jedną godzinę zajęć.");

        if (ects >= MinEcts && contactHours > ects * MaxHoursPerEcts)
            errors.Add($"Godziny zajęć ({contactHours} h) przekraczają łączny nakład pracy dla {ects} ECTS (maks. {ects * MaxHoursPerEcts} h).");

        return errors;
    }

    /// <summary>
    /// Sprawdza, czy suma nakładu pracy studenta zgadza się z punktami ECTS.
    /// Zwraca null, gdy wszystko jest w porządku, albo treść ostrzeżenia.
    /// </summary>
    public static string? CheckWorkload(int ects, int totalWorkloadHours)
    {
        var min = ects * MinHoursPerEcts;
        var max = ects * MaxHoursPerEcts;
        if (totalWorkloadHours < min || totalWorkloadHours > max)
            return $"Łączny nakład pracy ({totalWorkloadHours} h) nie odpowiada {ects} ECTS (oczekiwane {min}-{max} h).";
        return null;
    }
}
