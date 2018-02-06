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

namespace FlightTracker
{
    /// <summary>
    /// Interaction logic for SelectWindow.xaml
    /// 
    /// Left To Do:
    /// 1. Match Airline Codes to Images
    /// 2. Match SelectWindow Data to flightsearch
    /// 3. Comment Code, change names
    /// 4. Sort out folder design
    /// 5. Change WPF design to implement panels
    /// 
    /// </summary>
    public partial class SelectWindow : Window
    {
        private MainWindow main;
        private string choice;
        public SelectWindow(string choice, MainWindow m)
        {
            this.main = m;
            this.choice = choice;

            InitializeComponent();
            lblEnter.Content += $" {Application.Current.Resources[this.choice].ToString()}";
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ErrorMessage(bool isEmpty)
        {
            if (!isEmpty)
            {
                MessageBoxResult result = MessageBox.Show($"{Application.Current.Resources[this.choice].ToString()} {Application.Current.Resources["errorNoFlights"].ToString()}", Application.Current.Resources["errorTitle"].ToString(), MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show($"{Application.Current.Resources[this.choice].ToString()} {Application.Current.Resources["errorEmpty"].ToString()}", Application.Current.Resources["errorTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            /* WIP Getting Code
            string airportCode = txBoxChoice.Text;
            bool isAirportCode = false;

            ErrorMessage(true);
            */

            /* FOR TESTING */

            this.main.Hide();
            this.Close();

            FlightsWindow flight = new FlightsWindow(main, "EIDW");
            flight.Show();


        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        private void btnSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            Animation.Animate(TextBlock.PaddingProperty, new Thickness(130, 3, 0, 0), sender as TextBlock, new TimeSpan(0, 0, 0, 0, 200), preAnim: (o) => { btnSearch.Text = $"...{Application.Current.Resources["search"].ToString()}"; });
        }

        private void btnSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            Animation.Animate(TextBlock.PaddingProperty, new Thickness(5, 3, 0, 0), sender as TextBlock, new TimeSpan(0, 0, 0, 0, 200), preAnim: (o) => { btnSearch.Text = Application.Current.Resources["search"].ToString(); });
        }
    }
}
