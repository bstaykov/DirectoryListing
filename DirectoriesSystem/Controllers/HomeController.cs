namespace DirectoriesSystem.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    using DirectoriesSystem.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string relativePath = Server.MapPath(@"~\Files");

            try
            {
                if (Directory.Exists(relativePath))
                {
                    var browseViewModel = new BrowseViewModel()
                    {
                        Files = Directory.GetFiles(relativePath),
                        Directories = Directory.GetDirectories(relativePath),
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
    }
}