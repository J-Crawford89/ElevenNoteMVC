using ElevenNote.Data;
using ElevenNote.Models.NoteModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            return new NoteService(userId);
        }

        // GET: Note
        public ActionResult Index()
        {
            var noteService = CreateNoteService();
            var model = noteService.GetNotes();

            return View(model);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var noteService = CreateNoteService();
            if (noteService.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Note could not be created.");
            return View(model);
        }

        // GET: Note/Details/{id}
        public ActionResult Details(int id)
        {
            var noteService = CreateNoteService();
            var model = noteService.GetNoteById(id);

            return View(model);
        }
        
        // GET: Note/Delete/{id}
        public ActionResult Delete(int id)
        {
            var noteService = CreateNoteService();
            var model = noteService.GetNoteById(id);

            return View(model);
        }

        // POST: Note/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var noteService = CreateNoteService();
            noteService.DeleteNote(id);
            TempData["SaveResult"] = "Your note was deleted.";
            return RedirectToAction("Index");
        }

        // GET: Note/Edit/{id}
        public ActionResult Edit (int id)
        {
            var noteService = CreateNoteService();
            var detail = noteService.GetNoteById(id);
            var model = new NoteEdit
            {
                NoteId = detail.NoteId,
                Title = detail.Title,
                Content = detail.Content
            };
            return View(model);
        }

        // POST: Note/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);
            if (model.NoteId != id)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }
            var noteService = CreateNoteService();
            if (noteService.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Your note could not be updated.");
            return View(model);
        }
    }
}