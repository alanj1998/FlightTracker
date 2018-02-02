using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;

namespace FlightTracker
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private static Dictionary<string, string> languagesAvailable = JSON.GetJSONData<Dictionary<string, string>>(AppPaths.Path.LanguageList);
        public SettingsWindow()
        {
            InitializeComponent();
            SetUpSettingsWindow();
            cmBoxLanguage.ItemsSource = languagesAvailable.Select(x => x.Value);
        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        public void SetUpSettingsWindow()
        {
            //Setup the language
            cmBoxLanguage.SelectedItem = languagesAvailable[UserSettings.Language];

            LinearGradientBrush red = SetGradientColor();
            

            if (UserSettings.Time == "24h")
                btn24h.Background = red;
            else
                btn12h.Background = red;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string chosenLanguage = cmBoxLanguage.SelectedItem as string;
            string language = languagesAvailable.FirstOrDefault(x => x.Value == chosenLanguage).Key;
            LinearGradientBrush red = SetGradientColor();

            if (btn12h.Background == red && UserSettings.Time != "12h")
                UserSettings.UpdateUserSettings("12h", Settings.Time.ToString());
            else if (btn24h.Background == red && UserSettings.Time != "24h")
                UserSettings.UpdateUserSettings("24h", Settings.Time.ToString());
            
            if(language != UserSettings.Language)
            {
                bool success = UserSettings.UpdateUserSettings(language, Settings.Language.ToString());

                if (success)
                {
                    cmBoxLanguage.SelectedItem = languagesAvailable[UserSettings.Language];
                    AppSettings.SetResourceFile(language);
                    MessageBox.Show(Application.Current.Resources["successSave"].ToString(), Application.Current.Resources["success"].ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                    Dispatcher.Invoke(() =>
                    {
                        TextBlock t = Application.Current.MainWindow.FindName("lblOptions") as TextBlock;
                        t.Text = Application.Current.Resources["clickMore"] as string;
                    });
                    this.Close();
                }
            }

                            
        }

        private LinearGradientBrush SetGradientColor()
        {
            LinearGradientBrush red = new LinearGradientBrush();

            red.StartPoint = new Point(0.5, 0);
            red.EndPoint = new Point(0.5, 1);
            red.GradientStops.Add(new GradientStop(Color.FromRgb(236, 60, 60), 0.671));
            red.GradientStops.Add(new GradientStop(Color.FromRgb(224, 0, 0), 0.936));

            return red;
        }
    }
}
