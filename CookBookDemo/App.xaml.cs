using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Cookbook.ViewModels;
using Cookbook.ObjectModels;
using Windows.UI.ViewManagement;
using Windows.UI.Core;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=402347&clcid=0x409

namespace Cookbook
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Allows tracking page views, exceptions and other telemetry through the Microsoft Application Insights service.
        /// </summary>
        //For custom event
        public static TelemetryClient TelemetryClient;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
              Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
              Microsoft.ApplicationInsights.WindowsCollectors.Session);
            //For custom event
            TelemetryClient = new TelemetryClient();
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            ApplicationView.GetForCurrentView().SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            //#if DEBUG
            //            if (System.Diagnostics.Debugger.IsAttached)
            //            {
            //                this.DebugSettings.EnableFrameRateCounter = true;
            //            }
            //#endif
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Color lightTheme = new Windows.UI.Color();
            Windows.UI.Color darkTheme = new Windows.UI.Color();
            lightTheme = Windows.UI.Color.FromArgb(200, 237, 223, 192);
            darkTheme = Windows.UI.Color.FromArgb(200, 95, 71, 73);


            //Make titlebar look nicer
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonForegroundColor = darkTheme;
            titleBar.ButtonBackgroundColor = lightTheme;
            titleBar.ButtonHoverBackgroundColor = darkTheme;
            titleBar.ButtonHoverForegroundColor = lightTheme;
            titleBar.ButtonPressedBackgroundColor = lightTheme;
            titleBar.ButtonPressedForegroundColor = darkTheme;
            titleBar.BackgroundColor = lightTheme;


            //Hook up back buttons
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, f) =>
            {
                if (rootFrame.CanGoBack)
                    f.Handled = true;
                    rootFrame.GoBack();
            };

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                
                rootFrame.Navigate(typeof(Categories), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
