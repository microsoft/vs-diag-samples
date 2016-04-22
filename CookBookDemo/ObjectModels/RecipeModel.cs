using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.UI.Xaml.Controls;
using Windows.Data.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Windows.Storage;
using System.IO;
using Cookbook.ViewModels;

namespace Cookbook.ObjectModels
{
    public class RecipeModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }            
        }

        [JsonProperty(PropertyName = "id")]
        private int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        //Defines the recipe title
        [JsonProperty(PropertyName = "title")]
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        //Defines the recipe serving size
        [JsonProperty(PropertyName = "servingSize")]
        private int servingSize;
        public int ServingSize
        {
            get { return servingSize; }
            set
            {
                servingSize = value;
                OnPropertyChanged("ServingSize");
            }
        }

        //Defines each ingredient as a string
        [JsonProperty(PropertyName = "ingredientsList")]
        private ObservableCollection<string> ingredientsList;
        public ObservableCollection<string> IngredientsList
        {
            get {
                //for (int i = 0; i < 100000000; i++) { }
                return ingredientsList;
            }
            set
            {
                ingredientsList = value;
                OnPropertyChanged("IngredientsList");
            }
        }

        //Defines the picture to be displayed for each recipe
        [JsonProperty(PropertyName = "recipePicturePath")]
        private string recipePicturePath;
        public string RecipePicturePath
        {
            get { return recipePicturePath; }
            set
            {
                recipePicturePath = value;
                OnPropertyChanged("RecipePicturePath");
            }
        }

        //Defines the amount of time it takes to prepare the recipe
        [JsonProperty(PropertyName = "prepTime")]
        private TimeSpan prepTime;
        public TimeSpan PrepTime
        {
            get { return prepTime; }
            set
            {
                prepTime = value;
                OnPropertyChanged("PrepTime");
            }
        }

        //Defines the amount of time it takes to cook the recipe
        [JsonProperty(PropertyName = "cookTime")]
        private TimeSpan cookTime;
        public TimeSpan CookTime
        {
            get { return cookTime; }
            set
            {
                cookTime = value;
                OnPropertyChanged("CookTime");
            }
        }

        //Defines the amount of time it takes to prepare, cook, and serve the recipe
        [JsonProperty(PropertyName = "readyInTime")]
        private TimeSpan readyInTime;
        public TimeSpan ReadyInTime
        {
            get { return readyInTime; }
            set
            {
                readyInTime = value;
                OnPropertyChanged("ReadyInTime");
            }
        }

        //Gives more information about a specific recipe when the user clicks "About this recipe"
        [JsonProperty(PropertyName = "aboutThisRecipe")]
        private string aboutThisRecipe;
        public string AboutThisRecipe
        {
            get { return aboutThisRecipe; }
            set
            {
                aboutThisRecipe = value;
                OnPropertyChanged("AboutThisRecipe");
            }
        }

        //Gives nutrition info about each recipe: each int will represent one nutrition (e.g. first element of list is calories, second is carbs...)
        [JsonProperty(PropertyName = "nutritionInfo")]
        private ObservableCollection<int> nutritionInfo;
        public ObservableCollection<int> NutritionInfo
        {
            get { return nutritionInfo; }
            set
            {
                nutritionInfo = value;
                OnPropertyChanged("NutritionInfo");
            }
        }

        //List out each cooking step as a string. 
        [JsonProperty(PropertyName = "cookingSteps")]
        private ObservableCollection<Instruction> cookingSteps;
        public ObservableCollection<Instruction> CookingSteps
        {
            get { return cookingSteps; }
            set
            {
                cookingSteps = value;
                OnPropertyChanged("CookingSteps");
            }
        }

        //Determines which category this recipe belongs in (e.g. Seafood, Dessert, etc...)
        [JsonProperty(PropertyName = "category")]
        private string category;
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }

        //Return the title of the recipe
        public override string ToString()
        {
            return this.Title;
        }
    }
    public class CategoryItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private string categoryName;
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value;
                OnPropertyChanged("CategoryName");
            }            
        }

        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        public CategoryItem(string name, string path)
        {
            this.CategoryName = name;
            this.ImagePath = path;
        }
    }

    //Class to store instruction data including number, text, and picture path. A collection of instructions will be used in a layout element on the RecipeInstructions view
    public class Instruction
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        [JsonProperty(PropertyName = "instructionNumber")]
        private int instructionNumber;
        public int InstructionNumber
        {
            get { return instructionNumber; }
            set
            {
                instructionNumber = value;
                OnPropertyChanged("InstructionNumber");
            }
        }

        [JsonProperty(PropertyName = "instructionText")]
        private string instructionText;
        public string InstructionText
        {
            get { return instructionText; }
            set
            {
                instructionText = value;
                OnPropertyChanged("InstructionText");
            }
        }

        [JsonProperty(PropertyName = "instructionImagePath")]
        private string instructionImagePath;
        public string InstructionImagePath
        {
            get { return instructionImagePath; }
            set
            {
                instructionImagePath = value;
                OnPropertyChanged("InstructionImagePath");
            }
        }

    }

    public sealed class RecipeDataSource
    {
        //We only want to parse the JSON once, this will satisfy that (we will always check to see if recipeList is null or not).
        public static ObservableCollection<RecipeModel> recipeList;
        
        //Returns the list of recipes from the Json file
        private static async Task<ObservableCollection<RecipeModel>> GetRecipesFromJsonAsync()
        {
            #region Web Call
            //Call webService for Data First
            WebData.GetData(); 
            #endregion

            //This returns the full file path of the stored recipe data
            Uri dataUri = new Uri("ms-appx:///Data/RecipeData.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            
            //Create a string to deserialize to the collection
            string resultJson = await FileIO.ReadTextAsync(file);

            //Return the ObservableCollection of the list of RecipeModels from the file            
            return JsonConvert.DeserializeObject<ObservableCollection<RecipeModel>>(resultJson);
        }

        //This method will populate the recipeList if it has not already happened, otherwise it will just return to the method calling it.
        private static async Task PopulateRecipesListAsync()
        {
            if(recipeList == null)
            {
                recipeList = await GetRecipesFromJsonAsync();
                return;
            }
            else
            {
                return;
            }
        }

        //Returns all categories with an associated picture for each cateogry returned
        public static async Task<ObservableCollection<CategoryItem>> GetAllCategoriesAsync()
        {
            await PopulateRecipesListAsync();
      
            List<string> categoriesList = new List<string>();
            ObservableCollection<CategoryItem> categories = new ObservableCollection<CategoryItem>();

            foreach (RecipeModel item in recipeList)
            {
                if (!categoriesList.Contains(item.Category))
                {                           
                    if(item.Category == "Meat & Poultry")
                    {
                        categories.Add(new CategoryItem(item.Category, "/Images/Layout/CategoriesIcons/Icon_MeatPoultry.png"));
                    }
                    else
                    {
                        categories.Add(new CategoryItem(item.Category, "/Images/Layout/CategoriesIcons/Icon_" + item.Category + ".png"));
                    }
                    categoriesList.Add(item.Category);
                }
            }
            return categories;
        }        

        //Returns all recipes with a specific cateogry
        public static async Task<ObservableCollection<RecipeModel>> GetRecipesByCategoryAsync(string category)
        {
            await PopulateRecipesListAsync();

            ObservableCollection<RecipeModel> sortedRecipes = new ObservableCollection<RecipeModel>();

            foreach(RecipeModel recipe in recipeList)
            {
                if(recipe.Category == category)
                {
                    sortedRecipes.Add(recipe);
                }
            }
            return sortedRecipes;
        }

        //Returns a single recipe with a specific name - this will return the first one encountered if there are duplicates
        public static async Task<RecipeModel> GetRecipeByTitleAsync(string title)
        {
            await PopulateRecipesListAsync();

            foreach(RecipeModel recipe in recipeList)
            {
                if(recipe.Title == title)
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}

