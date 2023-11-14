using System;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.Controllers
{
	public class NotFoundPageController:Controller
	{
		public IActionResult Index()
		{
			return View();
        }
	}
}

