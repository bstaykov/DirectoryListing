namespace DirectoriesSystem.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web.Mvc;

    using DirectoriesSystem.Models;

    public class HomeController : Controller
    {
        private const string RelativePath = "\\Files\\RootFolder\\";
        private const int RelativePathLength = 18;

        public ActionResult Index(string directory = "")
        {
            // TODO DELETE
            this.TempData["error"] = "ERROR1";

            string relativePath = Server.MapPath("~\\Files\\RootFolder\\");

            // TODO DELETE
            this.TempData["error"] = "ERROR2";

            // TODO DELETE
            if (directory == null)
            {
                this.TempData["error"] = "directory IS NULL ";
            }

            // TODO DELETE
            if (relativePath == null)
            {
                this.TempData["error"] = "relativePath IS NULL ";
            }

            //string relativePath = Server.MapPath("~" + RelativePath);
            string fullPath = relativePath + directory;

            // TODO DELETE
            this.TempData["error"] = "ERROR3";

            // TODO DELETE
            if (fullPath == null)
            {
                this.TempData["error"] = "fullPath IS NULL ";
            }

            try
            {
                if (Directory.Exists(fullPath))
                {
                    FileViewModel[] files;
                    DirectoryViewModel[] directories;

                    this.ExtractPathsInformation(fullPath, out files, out directories);

                    // TODO DELETE
                    this.TempData["error"] = "ERROR4";

                    // TODO DELETE
                    if (files == null)
                    {
                        this.TempData["error"] = "files IS NULL ";
                    }
                    // TODO DELETE
                    if (directories == null)
                    {
                        this.TempData["error"] = "directories IS NULL ";
                    }
                    // TODO DELETE
                    if (this.GetParentDirectory(directory) == null)
                    {
                        this.TempData["error"] = "this.GetParentDirectory(directory) IS NULL ";
                    }

                    var browseViewModel = new BrowseViewModel()
                    {
                        Files = files,
                        Directories = directories,
                        ParentDirectory = this.GetParentDirectory(directory),
                        CurrentDirectory = directory,
                    };

                    // TODO DELETE
                    this.TempData["error"] = "ERROR5";
                    // TODO DELETE
                    if (browseViewModel == null)
                    {
                        this.TempData["error"] = "browseViewModel IS NULL ";
                    }

                    return this.View(browseViewModel);
                }
            }
            catch (Exception)
            {
                // TODO DELETE
                this.TempData["error"] = "ERROR6";

                return this.View("Error");
            }

            // TODO DELETE
            this.TempData["error"] = "NOT EXISTING DIRECTORY" + fullPath;

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

        [HttpGet]
        public ActionResult AddDirectory(string currentDirectory)
        {
            if (currentDirectory == null)
            {
                currentDirectory = string.Empty;
            }

            string fullPath = Server.MapPath("~" + RelativePath + currentDirectory);

            if (Directory.Exists(fullPath))
            {
                return this.PartialView("_AddDirectory", new DirectoryInputModel() { CurrentDirectory = currentDirectory, Name = "NewFolder" });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDirectory(DirectoryInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return this.PartialView("_AddDirectory", model);
            }

            string fullPath = Server.MapPath("~" + RelativePath + model.CurrentDirectory + "\\" + model.Name);

            if (Directory.Exists(fullPath))
            {
                ModelState.AddModelError(string.Empty, "Error! Directory name duplicates!");
                return this.PartialView("_AddDirectory", model);
            }

            DirectoryInfo newDirectory = Directory.CreateDirectory(fullPath);

            if (newDirectory == null)
            {
                ModelState.AddModelError(string.Empty, "Error! Directory was not added!");
                return this.PartialView("_AddDirectory", model);
            }

            return this.PartialView("_NewDirectoryInfo", newDirectory);
        }

        [HttpGet]
        public ActionResult RenameDirectory(string directory)
        {
            if (directory == null || directory == string.Empty)
            {
                return null;
            }

            string fullPath = Server.MapPath("~" + RelativePath + directory);

            if (Directory.Exists(fullPath))
            {
                DirectoryInfo currentDirectory = new DirectoryInfo(fullPath);

                return this.PartialView("_EditDirectory", new DirectoryEditInputModel() { Name = currentDirectory.Name, ModifiedOn = currentDirectory.LastWriteTime, FullPath = directory });
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RenameDirectory(DirectoryEditInputModel model)
        {
            if (ModelState.IsValid == false)
            {
                return this.PartialView("_EditDirectory", model);
            }

            string fullPath = Server.MapPath("~" + RelativePath + model.FullPath);

            if (Directory.Exists(fullPath))
            {
                string newDirectory = fullPath.Substring(0, fullPath.LastIndexOf("\\") + 1) + model.Name;
                if (Directory.Exists(newDirectory))
                {
                    ModelState.AddModelError(string.Empty, "Error! Directory name duplicates!");
                    return this.PartialView("_EditDirectory", model);
                }

                Directory.Move(fullPath, newDirectory);
                DirectoryInfo newDirectoryInfo = new DirectoryInfo(newDirectory);
                if (newDirectoryInfo == null)
                {
                    ModelState.AddModelError(string.Empty, "Error! Directory was not moved!");
                    return this.PartialView("_EditDirectory", model);
                }

                return this.PartialView("_NewDirectoryInfo", newDirectoryInfo);
            }

            ModelState.AddModelError(string.Empty, "Error! Directory is missing!");
            return this.PartialView("_EditDirectory", model);
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
            if (directory != null)
            {
                int parentDirectoryEndIndex = directory.LastIndexOf("\\");
                if (parentDirectoryEndIndex != -1)
                {
                    string parentDirectory = directory.Substring(0, parentDirectoryEndIndex);

                    // TODO DELETE 
                    if (parentDirectory == null)
                    {
                        this.TempData["error"] = "parentDirectory IS NULL ";
                    }

                    return parentDirectory;
                }
            }

            return string.Empty;
        }
    }
}