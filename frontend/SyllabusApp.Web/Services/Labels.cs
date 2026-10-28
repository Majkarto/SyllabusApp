using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Web.Services;

public static class Labels
{
    public static string Status(SyllabusStatus status) => status switch
    {
        SyllabusStatus.Draft => "Szkic",
        SyllabusStatus.UnderReview => "W recenzji",
        SyllabusStatus.Approved => "Zatwierdzony",
        SyllabusStatus.Published => "Opublikowany",
        SyllabusStatus.Archived => "Zarchiwizowany",
        _ => status.ToString()
    };

    public static string Category(LearningOutcomeCategory category) => category switch
    {
        LearningOutcomeCategory.Knowledge => "Wiedza",
        LearningOutcomeCategory.Skills => "Umiejętności",
        LearningOutcomeCategory.Social => "Kompetencje społeczne",
        _ => category.ToString()
    };

    public static string Literature(LiteratureType type) =>
        type == LiteratureType.Required ? "Podstawowa" : "Uzupełniająca";

    public static string Workload(WorkloadType type) => type switch
    {
        WorkloadType.Contact => "Zajęcia kontaktowe",
        WorkloadType.Independent => "Praca własna",
        WorkloadType.Assessment => "Zaliczenie",
        _ => type.ToString()
    };
}
