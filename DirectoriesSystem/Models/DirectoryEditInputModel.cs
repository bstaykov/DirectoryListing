namespace DirectoriesSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DirectoryEditInputModel
    {
        [Required(ErrorMessage = "Required!")]
        [StringLength(100, ErrorMessage = "Maximum 100 symbols.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Letters and numbers only!")]
        public string Name { get; set; }

        public string FullPath { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}