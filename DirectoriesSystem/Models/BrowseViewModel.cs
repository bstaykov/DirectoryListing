namespace DirectoriesSystem.Models
{
    public class BrowseViewModel
    {
        public string[] Files { get; set; }

        public string[] Directories { get; set; }

        public string ParentDirectory { get; set; }
    }
}