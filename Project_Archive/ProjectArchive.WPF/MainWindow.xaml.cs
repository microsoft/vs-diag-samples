//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

public enum ViewModelMode { Apps, ServerError, TestResults, ParallelMode, LotteryMode }

public class MainWindowViewModel : ViewModelBase
{
    public bool ShowInteropButton
    {
        get
        {
#if INTEROP
            return true;
#else
            return false;
#endif
        }
    }

    private List<AppInfo> m_apps;
    public List<AppInfo> Apps { get { return m_apps; } set { m_apps = value; OnPropertyChanged(); OnPropertyChanged("AppTypes"); OnPropertyChanged("ShowAppSelector"); } }
    List<AppTypeContainer> m_appTypes;
    public List<AppTypeContainer> AppTypes
    {
        get
        {
            if (m_appTypes != null)
            {
                return m_appTypes;
            }


            var appTypes = new List<AppTypeContainer>();
            if (this.Apps == null)
            {

                appTypes.Add(new AppTypeContainer() { AppType = "<None Available>" });
                return appTypes;
            }


            var types = this.Apps.Select(a => a.AppType).Distinct();
            appTypes.Add(new AppTypeContainer() { AppType = "[All]" });
            foreach (var t in types)
            {
                appTypes.Add(new AppTypeContainer() { AppType = t });
            }
            m_appTypes = appTypes;
            return m_appTypes;
        }
    }
    public bool ServerError
    {
        get
        {
            if (m_mode == ViewModelMode.ServerError)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private string m_greeting;
    public string Greeting { get { return m_greeting; } set { m_greeting = value; OnPropertyChanged(); } }

    private List<TestResult> m_testResults;
    public List<TestResult> TestResults { get { return m_testResults; } set { m_testResults = value; OnPropertyChanged(); } }

    public bool ShowAppSelector { get { if (this.Apps != null) { return true; } return false; } }
    public bool ShowTestResults { get { if (m_mode == ViewModelMode.TestResults) return true; else { return false; } } }
    public bool ShowApps { get { if (m_mode == ViewModelMode.Apps) return true; else { return false; } } }
    public bool ShowAppPanel { get { if (ShowApps || ServerError) { return true; } else { return false; } } }
    private ViewModelMode m_mode;
    public ViewModelMode Mode
    {
        get { return m_mode; }
        set
        {
            m_mode = value;

            OnPropertyChanged("ShowAppPanel");
            OnPropertyChanged("ShowApps");
            OnPropertyChanged("ShowTestResults");
            OnPropertyChanged("ServerError");
            OnPropertyChanged("ParallelMode");
            OnPropertyChanged("LotteryMode");
        }
    }

    public bool ParallelMode
    {
        get
        {
            if (m_mode == ViewModelMode.ParallelMode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool LotteryMode
    {
        get
        {
            if (m_mode == ViewModelMode.LotteryMode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    private List<int> m_primes;
    private string m_winnerStatus;

    public List<int> PrimeNumbers
    {
        get { return m_primes; }
        set { m_primes = value; OnPropertyChanged(); }
    }

    public enum WinnerStatus { Winner, Loser }
    public string WinnerStatusResult
    {
        get { return m_winnerStatus; }
        set { m_winnerStatus = value; OnPropertyChanged(); }
    }

}

public class StringProvider
{
    const string TitleParameter = "/conference";

    public string GetGreeting()
    {
        return "Hello";
    }


    public string GetConference()
    {
        var args = Environment.GetCommandLineArgs();
        string conf = $"[Provide {TitleParameter} argument]";
        for (int x = 1; x < args.Length; x++)
        {
            if (args[x].Equals(TitleParameter, StringComparison.OrdinalIgnoreCase))
            {
                conf = args[x + 1];
                break;
            }
        }
        return conf;
    }

    public string GetPunctuation()
    {
        return GetExclamationPoint();
    }

    public string GetExclamationPoint()
    {
        return "!";
    }

}

public class Game
{
    // Field
    public double userInputGuess;
    public double endingOutput;
    public bool isWinnerStatus;
    //public int delta;
    public Random random;
    public static int numIterations = 5;

    // Constructor that takes one argument.
    public Game(double guess)
    {
        userInputGuess = guess;
        endingOutput = guess;
        isWinnerStatus = false;
        //delta = 0;
        random = new Random();
    }


    public bool isWinner()
    {
        return isWinnerStatus;
    }

    public void runGame()
    {
        for (var i = 0; i < numIterations; i++)
        {
            //gravity should lean toward negative
            var delta = random.Next(-10, 5)/100;
            //var delta = (double)random.Next(-10, 5) / 100;
            endingOutput += delta;
        }
        if (endingOutput >= userInputGuess)
        {
            isWinnerStatus = true;
        }
        else
        {
            isWinnerStatus = false;
        }
    }
}

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    const string IISExpressUrl = "http://localhost:10531/";
    const string IISUrl = "http://localhost/";
    const string WebValueApiUrl = "api/Values";
    const string SlowWebValueApiUrl = "api/SlowValues";
    const string CertificateApiUrl = "api/Certificates";
    const string DataRootPath = @"..\..\..\ProjectArchive.Web\App_Data\";

    MainWindowViewModel ViewModel { get; set; }
    StringUtilities StringEncoder { get; set; }
    Dictionary<string, Certificate> CertificateStore { get; set; }
    string CurrentAppJson { get; set; }
    List<AppInfo> CachedApps { get; set; }
    Game game;

    public MainWindow()
    {        
        InitializeComponent();     
        var sp = new StringProvider();
        ViewModel = new MainWindowViewModel();
        ViewModel.Greeting = string.Concat(sp.GetGreeting(), " ", sp.GetConference(), sp.GetPunctuation());
        this.DataContext = ViewModel;

        this.StringEncoder = new StringUtilities(EncodingFormats.ASCII);
        this.CertificateStore = new Dictionary<string, Certificate>();
    }

    private async void GetAllApps_Click(object sender, RoutedEventArgs e)
    {
        var url = IISExpressUrl + WebValueApiUrl;
        await GetAppsFromWebAsync(url);
    }

    private async void GetBadUrl_Click(object sender, RoutedEventArgs e)
    {
        var url = IISUrl + WebValueApiUrl;
        await GetAppsFromWebAsync(url);
    }

    private void LoadData_Click(object sender, RoutedEventArgs e)
    {
        var dataprovider = new DataSource(DataRootPath);
        List<AppInfo> apps = dataprovider.Applications;

        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        foreach (var app in apps)
        {
            var languages = app.GetLanguagesForDisplay();
            Debug.Assert(languages != null);
        }

        AppInfo[] randomApps = new AppInfo[10];
        foreach( var app in apps)
        {
            Random random = new Random();
            var randomNumber = random.Next(0, 1000);
            var randomIndex = (app.AppID + randomNumber) % 10;
            randomApps[randomIndex] = app;
        }
    }

    private void RandomCalculation_Click(object sender, RoutedEventArgs e)
    {
        this.ViewModel.Mode = ViewModelMode.LotteryMode;

    }

    private void RunTests_Click(object sender, RoutedEventArgs e)
    {
        var typesToTest = new string[] { "Web", "Mobile", "desktop" };

        AppDataProvider dataProvider = new AppDataProvider(DataRootPath);
        var enc = new StringUtilities(EncodingFormats.Base64);
        var allApps = dataProvider.GetAllApps();
        var results = new List<TestResult>();
        var cert = Certificate.GenerateNewCertificate();

        foreach (var appType in typesToTest)
        {
            var encodedType = enc.EncodeString(appType, cert);
            var apps = dataProvider.GetAppsByType(encodedType);
            var appsOfType = allApps.Where(a => a.AppType.Equals(appType, StringComparison.OrdinalIgnoreCase)).ToList();

            var result = new TestResult(appType, apps.Count == appsOfType.Count);
            results.Add(result);
        }

        ViewModel.TestResults = results;
        ViewModel.Mode = ViewModelMode.TestResults;
    }

    private async void LoadByType_Click(object sender, RoutedEventArgs e)
    {
        var container = cmbItemType.SelectedValue as AppTypeContainer;
        var value = container.AppType;
        var cert = await GetServerCertificateAsync(IISExpressUrl);

        if (value.Equals("[All]", StringComparison.OrdinalIgnoreCase))
        {
            GetAllApps_Click(sender, e);
        }
        else if (!value.Contains("<"))
        {
            var encodedValue = StringEncoder.EncodeString(value, cert);
            var url = IISExpressUrl + WebValueApiUrl + $"?encodedType={encodedValue}";
            await GetAppsFromWebAsync(url);
        }

    }

    private void ClearAppList_Click(object sender, RoutedEventArgs e)
    {
        this.ViewModel.Apps = null;
    }

    private async Task GetAppsFromWebAsync(string url)
    {
        ViewModel.Apps = null;

        try
        {
            var json = await WebUtilities.Get(url);
            List<AppInfo> apps = StringUtilities.DeserializeApps(json);
            var appInfoContainer = new AppInfoContainer(apps);
            cmbItemType.SelectionChanged += appInfoContainer.CmbItemType_SelectionChanged;
            ViewModel.Apps = appInfoContainer.Apps;
            ViewModel.Mode = ViewModelMode.Apps;
        }
        catch (Exception)
        {
            ViewModel.Mode = ViewModelMode.ServerError;
        }
    }

    private List<AppInfo> DeserializeOrGetCachedApps(string appJson)
    {
        if (appJson != this.CurrentAppJson || this.CachedApps == null)
        {
            this.CurrentAppJson = appJson;
            this.CachedApps = StringUtilities.DeserializeApps(appJson);
        }

        return this.CachedApps;
    }

    private void Parallel_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            SettingsLibrary.FileUtilities usersSettings = new SettingsLibrary.FileUtilities();
            this.ViewModel.Mode = ViewModelMode.ParallelMode;
            ParallelSetupHelper parallelInitializer = new ParallelSetupHelper();
                parallelInitializer.Initialize().HelpSimplify().Setup();
        }
        catch (Exception ex)
        {
            this.ViewModel.Mode = ViewModelMode.ServerError;
        }
    }

    public class ParallelSetupHelper
    {
        public ParallelSimplifier Initialize()
        {
            return new ParallelSimplifier();
        }
    }

    public class ParallelSimplifier
    {
        public ParallelSimplifier HelpSimplify()
        {
             return null;  //TODO figure out what to return
           
        }

        public void Setup()
        {
            return;
        }
    }

    private async void CalculatePrimes_Click(object sender, RoutedEventArgs e)
    {
        int min = int.Parse(txtPrimeBegin.Text);
        int max = int.Parse(txtPrimeEnd.Text);

        this.ViewModel.PrimeNumbers = await NumberUtilities.CalculatePrimesAsync(min, max);
    }

    private void ResetWinnerStatusText_Click(object sender, RoutedEventArgs e)
    {
        this.ViewModel.WinnerStatusResult = "";
    }
    // Handler for Try My Luck button
    private void CalculateIfWinner_Click(object sender, RoutedEventArgs e)
    {
        this.game = new Game(int.Parse(userGuess.Text));
        game.runGame();
        if (game.isWinner())
        {
            this.ViewModel.WinnerStatusResult = "Winner";
        }
        else
        {
            this.ViewModel.WinnerStatusResult = "Loser";
        }
    }



    private void Interop_Click(object sender, RoutedEventArgs e)
    {
        NativeMethods.DoWork();
        Debugger.Break();
    }

    private async Task<Certificate> GetServerCertificateAsync(string baseUrl)
    {
        if (this.CertificateStore.ContainsKey(baseUrl))
        {
            return this.CertificateStore[baseUrl];
        }
        else
        {
            var certJson = Certificate.GetNewCertificateJson();
            var base64cert = StringUtilities.ConvertToBase64(certJson);
            var url = baseUrl + CertificateApiUrl;
            var serverCert = await WebUtilities.GetCertificate(url);
            return serverCert;
        }
    }
}


