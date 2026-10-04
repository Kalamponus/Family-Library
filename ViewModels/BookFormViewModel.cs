using System.ComponentModel.DataAnnotations;

namespace FamilyLibrary.ViewModels
{
    public class BookFormViewModel
    {
        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(255, ErrorMessage = "Название не должно превышать 255 символов")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Автор обязателен")]
        [StringLength(255, ErrorMessage = "Имя автора не должно превышать 255 символов")]
        public string Author { get; set; } = "";

        [Required(ErrorMessage = "Дата издания обязательна")]
        public DateOnly ReleaseDate { get; set; }

        public string? Description { get; set; } = "";
        public string? TableOfContents { get; set; } = "";
    }
}
