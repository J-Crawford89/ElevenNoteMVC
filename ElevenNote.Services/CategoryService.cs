using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;
        private readonly ApplicationDbContext _ctx = new ApplicationDbContext();

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };
            _ctx.Categories.Add(entity);
            return _ctx.SaveChanges() == 1;
        }

        public IEnumerable<CategoryListItem> GetCategories()
        {
            var categoryArray = _ctx.Categories.Select(e => new CategoryListItem
            {
                CategoryId = e.CategoryId,
                Name = e.Name
            }).ToArray();

            return categoryArray;
        }
        
        public CategoryDetail GetCategoryById(int id)
        {
            var entity = _ctx.Categories.Single(e => e.CategoryId == id);
            return new CategoryDetail
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name
            };
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            var entity = _ctx.Categories.Single(e => e.CategoryId == model.CategoryId);

            entity.Name = model.Name;

            return _ctx.SaveChanges() == 1;
        }

        public bool DeleteCategory(int id)
        {
            var entity = _ctx.Categories.Single(e => e.CategoryId == id);
            _ctx.Categories.Remove(entity);

            return _ctx.SaveChanges() == 1;
        }
    }
}
