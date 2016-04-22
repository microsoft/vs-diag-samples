using Cookbook.ObjectModels;
using Cookbook.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Cookbook
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Recipes : Page
    {
        private RecipesViewModel recipesViewModel;

        public Recipes()
        {
            this.InitializeComponent();
            this.recipesViewModel = new RecipesViewModel();
        }


        public RecipesViewModel RecipesViewModel
        {
            get { return this.recipesViewModel; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string categoryName = e.Parameter as string;
            
            App.TelemetryClient.TrackEvent("OnNavigatedTo",
                new Dictionary<string, string>() { { "Categories", categoryName } });

            if (categoryName != null)
            {
                await recipesViewModel.GetAllRecipesByCategoryAsync(categoryName);
                this.DataContext = recipesViewModel;
            }

            //Show UI Back button if necessary
            if (Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

            //Memory Leak
            byte[] memoryLeak = new byte[10000000];
        }     
    }
}
