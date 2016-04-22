using Cookbook.ObjectModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.ViewModels
{
    public class CategoriesViewModel
    {
        private RecipesViewModel recipesVM;

        private ObservableCollection<CategoryItem> allCategories;

        public CategoriesViewModel()
        {
            recipesVM = new RecipesViewModel();
        }

        public ObservableCollection<CategoryItem> AllCategories
        {
            get { return this.allCategories; }
        }
        
        public async Task GetAllCategoriesAsync()
        {
            this.allCategories = await RecipeDataSource.GetAllCategoriesAsync();
        }
    }
}
