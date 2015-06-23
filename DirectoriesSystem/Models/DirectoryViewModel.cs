namespace DirectoriesSystem.Models
{
    using System;

    public class DirectoryViewModel
    {
        public string FullPath { get; set; }

        public string Name { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}