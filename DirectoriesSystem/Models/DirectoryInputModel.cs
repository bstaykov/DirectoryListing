namespace DirectoriesSystem.Models
{
    using System.ComponentModel.DataAnnotations;

    public class DirectoryInputModel
    {
        [Required(ErrorMessage = "Required!")]
        [StringLength(100, ErrorMessage = "Maximum 100 symbols.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only!")]
        public string Name { get; set; }

        public string CurrentDirectory { get; set; }
    }
}