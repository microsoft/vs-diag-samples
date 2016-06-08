using Microsoft.Win32;
using MyShuttle.Client.Core.DocumentResponse;
using MyShuttle.Client.Desktop.Core.ServiceAgents.Cache;
using MyShuttle.Client.Desktop.Infrastructure;
using MyShuttle.Client.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShuttle.Client.Desktop.Views
{
    /// <summary>
    /// Interaction logic for EditDriverViewxaml.xaml
    /// </summary>
    public partial class EditDriverView : Page, INotifyPropertyChanged
    {
        private string m_backingJson;
        public string BackingJsonFile
        {
            get { return m_backingJson; }
            set { m_backingJson = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public EditDriverView()
        {
            InitializeComponent();

            string backingJson = Directory.GetCurrentDirectory() + @"\..\..\..\MyShuttle.Diagnostics.Service\App_Data\drivers.json";
            if (File.Exists(backingJson))
            {
                var file = new FileInfo(backingJson);
                BackingJsonFile = file.FullName;
                lblJsonFile.Content = BackingJsonFile;
            }
        }

        private void btnBrowseForImage_Click(object sender, RoutedEventArgs e)
        {
            string selectedFile = browseForFile();
            updateDriverImage(selectedFile);
        }

        private void btnUserPlacholder_Click(object sender, RoutedEventArgs e)
        {
            string selectedFile = selectedFile = Directory.GetCurrentDirectory() + @"\..\..\Assets\driver_unknow_icon.png";
            updateDriverImage(selectedFile);
        }

        private void updateDriverImage(string selectedFile)
        {
            if (selectedFile != null)
            {
                Driver d = EditDriverViewModel.CurrentDriver;
                d.Picture = getSelectedBytes(selectedFile);
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            string selectedFile = browseForFile();
            if (selectedFile != null)
            {
                BackingJsonFile = selectedFile;
            }
        }

        private string browseForFile()
        {
            var dialog = new OpenFileDialog();
            var result = dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileName;
            }
            return null;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Driver d = EditDriverViewModel.CurrentDriver;
            WebRequestCache<Driver>.Add(d.DriverId.ToString(), d);

            if (BackingJsonFile == null && chkUpdateJson.IsChecked == true)
            {
                MessageBox.Show("No backing json file selected");
                return;
            }
            else if(chkUpdateJson.IsChecked == true)
            {
                WebRequestCache<Driver>.WriteCacheToFile(BackingJsonFile);
            }

            NavigationHelper.NavigateBack();
        }

        private byte[] getSelectedBytes(string file)
        {
            //string file = @"C:\Users\andrehal\Pictures\HeadShot.png";
            Uri uri = new Uri(file);

            // Open a Stream and decode a PNG image
            Stream imageStreamSource = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            // Draw the Image
            Image myImage = pic;//new Image();
            myImage.Source = bitmapSource;
            myImage.Stretch = Stretch.Fill;

            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(decoder.Frames[0]);
            encoder.Save(memStream);
            var bytes = memStream.GetBuffer();
            return bytes;
        }

    }
}
