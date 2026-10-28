using SyllabusApp.Domain.Entities;

namespace SyllabusApp.Web.Services;

/// <summary>Polskie nazwy wartości enumów wyświetlane w interfejsie.</summary>
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
}
