using ElevenNote.Models.CategoryModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        public CategoryService CreateCategoryService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return new CategoryService(userId);
        }
        // GET: Category
        public ActionResult Index()
        {
            var service = CreateCategoryService();
            var model = service.GetCategories();

            return View(model);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var service = CreateCategoryService();
            if(service.CreateCategory(model))
            {
                TempData["SaveResult"] = "Your category was created.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Your category could not be created.");
            return View(model);
        }

        // GET: Category/Details/{id}
        public ActionResult Details(int id)
        {
            var service = CreateCategoryService();
            var model = service.GetCategoryById(id);

            return View(model);
        }

        // GET: Category/Edit/{id}
        public ActionResult Edit(int id)
        {
            var service = CreateCategoryService();
            var detail = service.GetCategoryById(id);
            var model = new CategoryEdit
            {
                CategoryId = detail.CategoryId,
                Name = detail.Name
            };
            return View(model);
        }

        // POST: Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (model.CategoryId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }
            var service = CreateCategoryService();
            if (service.UpdateCategory(model))
            {
                TempData["SaveResult"] = "Your category was updated.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Your category could not be updated.");
            return View(model);
        }

        // GET: Category/Delete/{id}
        public ActionResult Delete(int id)
        {
            var service = CreateCategoryService();
            var model = service.GetCategoryById(id);

            return View(model);
        }

        // POST: Category/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateCategoryService();
            service.DeleteCategory(id);
            TempData["SaveResult"] = "Your category was deleted.";
            return RedirectToAction("Index");
        }
    }
}