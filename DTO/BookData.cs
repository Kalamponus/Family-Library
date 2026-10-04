namespace FamilyLibrary.DTO
{
    public record BookData(
        string Title,
        string? Description,
        string Author,
        DateOnly ReleaseDate,
        string? TableOfContents);
}
