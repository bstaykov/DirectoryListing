namespace DirectoriesSystem.Models
{
    public class BrowseViewModel
    {
        public FileViewModel[] Files { get; set; }

        public DirectoryViewModel[] Directories { get; set; }

        public string ParentDirectory { get; set; }
    }
}