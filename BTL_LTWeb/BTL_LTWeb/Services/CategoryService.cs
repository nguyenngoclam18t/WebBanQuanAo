using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTL_LTWeb.Models;

namespace BTL_LTWeb.Services
{
    public class CategoryService : BaseService
    {
        public List<Category> GetCategories()
        {
            return db.Categories.ToList();
        }

       


    }
}