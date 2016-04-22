using Cookbook.ObjectModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace Cookbook.ViewModels
{
    public class RecipeHandler
    {
        
        //This was to get the JSON setup such that it can serialize an observable collection of a list of recipe models!
        public async static Task WriteRecipe()
        {
            RecipeModel myRecipe = new RecipeModel();
            myRecipe.Id = 100;
            myRecipe.Title = "Grilled Oysters on the Half-shell with Pancetta and Barbecue Sauce";
            myRecipe.ServingSize = 4;
            myRecipe.IngredientsList = new ObservableCollection<string>();
            myRecipe.IngredientsList.Add("8oz pancetta, diced small");
            myRecipe.IngredientsList.Add("3 tablespoons butter");
            myRecipe.IngredientsList.Add("1/4 cup ketchup");
            myRecipe.IngredientsList.Add("Juice of 1 lemon (about 1/4 cup)");
            myRecipe.IngredientsList.Add("2 tablespoons roughly chopped fresh parsley");
            myRecipe.IngredientsList.Add("6 dashes Tobasco sauce");
            myRecipe.IngredientsList.Add("3 dashes Worcestershire sauce");
            myRecipe.IngredientsList.Add("Kosher salt and freshly cracked black pepper to taste");
            myRecipe.IngredientsList.Add("24 oysters of your choice, shells scrubbed");
            myRecipe.RecipePicturePath = "Assets/Logo.scale-100.png";
            myRecipe.PrepTime = new TimeSpan(0,30,0);
            myRecipe.CookTime = new TimeSpan(0, 15, 0);
            myRecipe.ReadyInTime = new TimeSpan(0, 60, 0);
            myRecipe.AboutThisRecipe = "Lorem ipsum dolor";
            myRecipe.NutritionInfo = new ObservableCollection<int>();
            myRecipe.NutritionInfo.Add(600);
            myRecipe.NutritionInfo.Add(30);
            myRecipe.NutritionInfo.Add(20);
            myRecipe.NutritionInfo.Add(15);
            myRecipe.NutritionInfo.Add(10);
            //myRecipe.CookingSteps = new ObservableCollection<string>();
            //myRecipe.CookingSteps.Add("Build a fire in your grill. When the coals are all ignited, the fire has died down, and the temperature is medium, you're ready to cook.");
            //myRecipe.CookingSteps.Add("While you wait for the fire, cook the pancetta on the stove top in a large saute pan over medium-low heat, stirring frequently until it is crisp and brown (about 7 minutes). Transfer to paper towels to drain.");
            //myRecipe.CookingSteps.Add("Melt the butter in a small saucepan.Whisk in the ketchup, lemon juice, half of the parsley(reserve the rest for garnishing), Tabasco, Worcestershire sauce, and salt and pepper, and remove from the heat.");
            //myRecipe.CookingSteps.Add("Shuck the oysters and loosen them in their bottom shells.Arrange the oysters(in their bottom shells) on the grill over the coals, and cook just until their edges start to ruffle(2 - 3 minutes).Spoon about 1 tablespoon sauce over each oyster, and continue to cook until the sauce is bubbling hot.Transfer to a serving platter, sprinkle with parsley and pancetta, and serve, but be carefuly -the oyster shells will be very hot.");
            //myRecipe.CookingStepsImagePaths = new ObservableCollection<string>();
            //myRecipe.CookingStepsImagePaths.Add("null");
            //myRecipe.CookingStepsImagePaths.Add("null");
            //myRecipe.CookingStepsImagePaths.Add("null");
            //myRecipe.CookingStepsImagePaths.Add("null");
            myRecipe.Category = "Appetizer";

            
            ObservableCollection<RecipeModel> recipesList = new ObservableCollection<RecipeModel>();
            recipesList.Add(myRecipe);
            recipesList.Add(myRecipe);
            recipesList.Add(myRecipe);
            recipesList.Add(myRecipe);

            
            string json = JsonConvert.SerializeObject(recipesList, Formatting.Indented);
            //IStorageFile myFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("Data\\RecipeData.txt");
            StorageFile myFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("RecipeData.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(myFile, json);
            string testString = myFile.Path;
        }

        //This method returns a list of RecipeModel objects for each recipe stored in RecipeData.txt
        public static async Task<ObservableCollection<List<RecipeModel>>> GetRecipesAsync()
        {
            //This returns the full file path of the stored recipe data
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("Data\\RecipeData.txt");
            string resultJson = await FileIO.ReadTextAsync(file);

            //Final list to be returned
            ObservableCollection<List<RecipeModel>> RecipesList = new ObservableCollection<List<RecipeModel>>();

            //Save each recipe data to a RecipeModel object
            return JsonConvert.DeserializeObject<ObservableCollection<List<RecipeModel>>>(resultJson);            
        }
    }
}
