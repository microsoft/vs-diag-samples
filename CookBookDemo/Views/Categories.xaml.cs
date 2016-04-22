using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Cookbook.ObjectModels;
using System.Collections.ObjectModel;
using Cookbook.ViewModels;
using Windows.UI.Core;
using Windows.Foundation.Metadata;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Cookbook
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Categories : Page
    {
        private CategoriesViewModel categoriesViewModel;

        public Categories()
        {
            this.InitializeComponent();
            this.categoriesViewModel = new CategoriesViewModel();
        }

        public CategoriesViewModel CategoriesViewModel
        {
            get { return this.categoriesViewModel; }
        }

        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Adaptive code to hide status bar
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            }
            await categoriesViewModel.GetAllCategoriesAsync();
            this.DataContext = categoriesViewModel;

            //Show UI Back button if necessary
            if (Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }

        }
    }
}
