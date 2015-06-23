namespace DirectoriesSystem.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;

    using DirectoriesSystem.Models;

    public class HomeController : Controller
    {
        private const string RelativePath = @"\Files\RootFolder\";
        private const int RelativePathLength = 18;

        public ActionResult Index(string directory)
        {
            string relativePath = Server.MapPath("~" + RelativePath);
            string fullPath = relativePath + directory;

            try
            {
                if (Directory.Exists(fullPath))
                {
                    FileViewModel[] files;
                    DirectoryViewModel[] directories;

                    this.ExtractPathsInformation(fullPath, out files, out directories);

                    var browseViewModel = new BrowseViewModel()
                    {
                        Files = files,
                        Directories = directories,
                        ParentDirectory = this.GetParentDirectory(directory),
                        CurrentDirectory = directory,
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

        public ActionResult DeleteDirectory(string directory)
        {
            string fullPath = Server.MapPath("~" + RelativePath + directory);

            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);

                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        private void ExtractPathsInformation(string relativePath, out FileViewModel[] filesViewModels, out DirectoryViewModel[] directoriesViewModels)
        {
            DirectoryInfo directory = new DirectoryInfo(relativePath);
            FileInfo[] filesInfo = directory.GetFiles();
            int filesCount = filesInfo.Length;
            filesViewModels = new FileViewModel[filesCount];
            for (int i = 0; i < filesCount; i++)
            {
                var fullPath = filesInfo[i].FullName;
                var path = fullPath.Substring(fullPath.IndexOf(RelativePath) + RelativePathLength);

                FileViewModel currentFileInfo = new FileViewModel()
                {
                    FullPath = path,
                    Name = filesInfo[i].Name,
                    ModifiedOn = filesInfo[i].LastWriteTime,
                    Size = filesInfo[i].Length.ToString(),
                };
                filesViewModels[i] = currentFileInfo;
            }

            DirectoryInfo[] directoriesInfo = directory.GetDirectories();
            int directoriesCount = directoriesInfo.Length;
            directoriesViewModels = new DirectoryViewModel[directoriesCount];
            for (int i = 0; i < directoriesCount; i++)
            {
                var fullPath = directoriesInfo[i].FullName;
                var path = fullPath.Substring(fullPath.IndexOf(RelativePath) + RelativePathLength);

                DirectoryViewModel currentDirectoryInfo = new DirectoryViewModel()
                {
                    FullPath = path,
                    Name = directoriesInfo[i].Name,
                    ModifiedOn = directoriesInfo[i].LastWriteTime,
                };

                directoriesViewModels[i] = currentDirectoryInfo;
            }
        }

        private string GetParentDirectory(string directory)
        {
            if (directory == null)
            {
                return null;
            }
            else
            {
                int parentDirectoryEndIndex = directory.LastIndexOf("\\");
                if (parentDirectoryEndIndex == -1)
                {
                    return string.Empty;
                }

                string parentDirectory = directory.Substring(0, directory.LastIndexOf("\\"));
                return parentDirectory;
            }
        }
    }
}