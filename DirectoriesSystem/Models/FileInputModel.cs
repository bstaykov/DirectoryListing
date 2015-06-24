namespace DirectoriesSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class FileInputModel
    {
        [Required(ErrorMessage = "Required!")]
        public HttpPostedFileBase File { get; set; }

        public string CurrentDirectory { get; set; }
    }
}