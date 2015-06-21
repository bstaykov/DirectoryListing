namespace DirectoriesSystem.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    using DirectoriesSystem.Models;

    public class HomeController : Controller
    {
        private const string RelativePath = @"\Files\RootFolder\";

        public ActionResult Index(string parentDirectory)
        {
            string relativePath = Server.MapPath("~" + RelativePath);

            try
            {
                if (Directory.Exists(relativePath))
                {
                    string[] files;
                    string[] directories;

                    ExtractPaths(relativePath, out files, out directories);

                    var browseViewModel = new BrowseViewModel()
                    {
                        Files = files,
                        Directories = directories,
                        ParentDirectory = parentDirectory,
                    };

                    return this.View(browseViewModel);
                }
            }
            catch (Exception)
            {
                return this.View("Error");
            }

            return this.View();
        }

        private void ExtractPaths(string relativePath, out string[] files, out string[] directories)
        {
            string[] fullFilesPaths = Directory.GetFiles(relativePath);
            string[] fullDirectoriesPaths = Directory.GetDirectories(relativePath);

            files = new string[fullFilesPaths.Length];
            directories = new string[fullDirectoriesPaths.Length];

            for (int i = 0; i < files.Length; i++)
            {
                files[i] = fullFilesPaths[i].Substring(fullFilesPaths[i].IndexOf(RelativePath) + RelativePath.Length);
            }

            for (int i = 0; i < directories.Length; i++)
            {
                directories[i] = fullDirectoriesPaths[i].Substring(fullDirectoriesPaths[i].IndexOf(RelativePath) + RelativePath.Length);
            }
        }
    }
}