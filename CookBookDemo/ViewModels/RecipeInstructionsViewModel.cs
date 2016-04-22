using Cookbook.ObjectModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Views
{
    public class RecipeInstructionsViewModel
    {
        private RecipeModel recipe = new RecipeModel();

        public RecipeModel Recipe
        {
            get { return this.recipe; }
        }

        private ObservableCollection<Instruction> instructions = new ObservableCollection<Instruction>();
        public ObservableCollection<Instruction> Instructions
        {
            get { return this.instructions; }
        }

        public async Task GetRecipeAndInstructions(string title)
        {
            this.recipe = await RecipeDataSource.GetRecipeByTitleAsync(title);    
        }
    }
}
