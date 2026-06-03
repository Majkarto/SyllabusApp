using SyllabusApp.Domain.Entities;
namespace SyllabusApp.Application.Dtos.Literatures;

public static class LiteraturesMappingExtensions
{
    public static LiteratureDto ToDto(this Literature entity)
    {
        return new LiteratureDto()
        {
            Id = entity.Id,
            Title = entity.Title,
            Author = entity.Author,
            Publisher = entity.Publisher,
            PublicationYear = entity.PublicationYear,
            Isbn = entity.Isbn,
            Type = entity.Type,
            SyllabusId = entity.SyllabusId,
        };
    }
    public static Literature ToEntity(this CreateLiteratureDto dto)
    {
        return new Literature
        {
            Title =  dto.Title,
            Author = dto.Author,
            Publisher = dto.Publisher,
            PublicationYear = dto.PublicationYear,
            Isbn = dto.Isbn,
            Type = dto.Type,
            SyllabusId = dto.SyllabusId
        };
    }
    public static void ApplyUpdate(this Literature entity, UpdateLiteratureDto dto)
    {
        entity.Title = dto.Title;
        entity.Author = dto.Author;
        entity.Publisher = dto.Publisher;
        entity.PublicationYear = dto.PublicationYear;
        entity.Isbn = dto.Isbn;
        entity.Type = dto.Type;
        entity.SyllabusId = dto.SyllabusId;
        entity.UpdatedAt = DateTime.UtcNow;
    }

}
    