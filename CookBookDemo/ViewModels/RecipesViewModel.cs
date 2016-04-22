using Cookbook.ObjectModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.ViewModels
{
    public class RecipesViewModel
    {
        private ObservableCollection<RecipeModel> allRecipesByCategory;

        public ObservableCollection<RecipeModel> AllRecipesByCategory
        {
            get { return this.allRecipesByCategory; }
        }
        
        public async Task GetAllRecipesByCategoryAsync(string categoryName)
        {
            this.allRecipesByCategory = await RecipeDataSource.GetRecipesByCategoryAsync(categoryName);
        }
    }
}
