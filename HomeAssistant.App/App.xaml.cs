using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HomeAssistant.App
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            SetLanguageDictionary();
        }

        private void SetLanguageDictionary()
        {
            ResourceDictionary dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-US":
                    dict.Source = new Uri("..\\Strings\\Dictionary_en.xaml", UriKind.Relative);
                    break;
                case "ru-RU":
                    dict.Source = new Uri("..\\Strings\\Dictionary_ru.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("..\\Strings\\Dictionary_en.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dict);
        }
    }
}
