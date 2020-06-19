using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.NoteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        private readonly ApplicationDbContext _ctx = new ApplicationDbContext();

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity = new Note()
            {
                OwnerId = _userId,
                Title = model.Title,
                Content = model.Content,
                CreatedUtc = DateTimeOffset.Now,
                CategoryId = model.CategoryId
            };
            _ctx.Notes.Add(entity);

            return _ctx.SaveChanges() == 1;
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            var noteArray = _ctx.Notes
                .Where(e => e.OwnerId == _userId)
                .Select(e => new NoteListItem
                {
                    NoteId = e.NoteId,
                    Title = e.Title,
                    CategoryName = e.Category.Name,
                    IsStarred = e.IsStarred,
                    CreatedUtc = e.CreatedUtc
                }).ToArray();
            return noteArray;
        }

        public NoteDetail GetNoteById(int id)
        {
            var entity = _ctx.Notes.Single(e => e.NoteId == id && e.OwnerId == _userId);
            return new NoteDetail
            {
                NoteId = entity.NoteId,
                Title = entity.Title,
                Content = entity.Content,
                CategoryName = entity.Category.Name,
                CategoryId = entity.CategoryId,
                CreatedUtc = entity.CreatedUtc,
                ModifiedUtc = entity.ModifiedUtc
            };
        }

        public bool UpdateNote(NoteEdit model)
        {
            var entity = _ctx.Notes.Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.CategoryId = model.CategoryId;
            entity.ModifiedUtc = DateTimeOffset.Now;
            entity.IsStarred = model.IsStarred;

            return _ctx.SaveChanges() == 1;
        }

        public bool DeleteNote(int noteId)
        {
            var entity = _ctx.Notes.Single(e => e.NoteId == noteId && e.OwnerId == _userId);

            _ctx.Notes.Remove(entity);

            return _ctx.SaveChanges() == 1;
        }
    }
}
