using System;
using System.Web.Mvc;

namespace MoneyManager.Controllers
{
	public class ImagesController : Controller
	{
		public ActionResult Index(string path)
		{
			var host = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

			var filePath = $"{host}\\Images\\{path}";

			if(!System.IO.File.Exists(filePath))
			{
				return File(System.IO.File.ReadAllBytes(AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + "\\image-not-found.jpg"), "image/jpeg");
			}

			byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

			return File(fileBytes, "image/jpeg");
		}
	}
}