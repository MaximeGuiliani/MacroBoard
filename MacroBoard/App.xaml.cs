using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MacroBoard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void Application_StartUp(Object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            //window.CurrentApplication = this;
            this.MainWindow = window;
            window.Show();
        }
        public enum ThemeTypes
        {
            Light, ColourfulLight,
            Dark, ColourfulDark
        }

        public static ThemeTypes CurrentTheme { get; set; }

        private static ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources.MergedDictionaries[0]; }
            set { Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        private static void ChangeTheme(Uri uri)
        {
            ThemeDictionary = new ResourceDictionary() { Source = uri };
        }

        public static void SetTheme(ThemeTypes theme)
        {
            string themeName = null;
            CurrentTheme = theme;
            switch (theme)
            {
                case ThemeTypes.Dark:themeName = "DarkTheme";break;
                case ThemeTypes.Light:themeName = "LightTheme";break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                    ChangeTheme(new Uri($"Themes/{themeName}.xaml", UriKind.Relative));
            }
            catch
            {
            }
        }
    }
}
