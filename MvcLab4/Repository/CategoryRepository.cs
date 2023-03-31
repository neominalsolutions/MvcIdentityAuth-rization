using MvcLab4.Entities;
using MvcLab4.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcLab4.Repository
{
    // use case is categoryrepository want to apply log after save operation
    public class CategoryRepository : EFCoreRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }


        public override int Save()
        {

            int result = base.Save();
            // some log operation 
            return result;

        }

        

        public List<Category> FilterByCategoryName(string cateoryName)
        {
            return _dbContext.Set<Category>().Where(x => x.CategoryName.Contains(cateoryName)).ToList();
        }
    }
}
