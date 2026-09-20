namespace FamilyLibrary.Models
{
    public record Book(
        int Id, 
        string Title, 
        string Description, 
        string Author, 
        DateOnly ReleaseDate, 
        string TableOfContents);
}
