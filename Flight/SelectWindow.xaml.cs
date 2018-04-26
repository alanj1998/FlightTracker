using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// </summary>
    public partial class SelectWindow : Window
    {
        private string choice;
        private string chosenICAO;
        public SelectWindow(string choice)
        {
            this.choice = choice;

            InitializeComponent();
            this.btnClose.DataContext = AppPaths.Path;
            lblEnter.Content += $" {Application.Current.Resources[this.choice].ToString()}";
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Method used to display error messages
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

        //Search button logic
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //If the user didn't choose a location
            if (!IsEnabled)
                MessageBox.Show("Pick Place from List First!", "Choose Place", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                string airportCode = this.chosenICAO;

                //When there is more than one char in the box
                if (airportCode.Length > 0)
                {
                    if (airportCode != "")
                    {
                        this.Owner.Hide();
                        this.Close();

                        FlightsWindow flight = new FlightsWindow(airportCode);
                        flight.Owner = this.Owner;
                        if (flight.IsSet)
                            flight.Show();
                        else
                        {
                            this.Close();
                            this.Owner.Show();
                        }
                    }
                    else
                    {
                        this.ErrorMessage(false);
                    }
                }
                else
                {
                    this.ErrorMessage(true);
                }
            }
        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        private void btnSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            Animation.Animate(TextBlock.PaddingProperty, new Thickness(200, 3, 0, 0), sender as TextBlock, new TimeSpan(0, 0, 0, 0, 200), preAnim: (o) => { btnSearch.Text = $"...{Application.Current.Resources["search"].ToString()}"; });
        }

        private void btnSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            Animation.Animate(TextBlock.PaddingProperty, new Thickness(5, 3, 0, 0), sender as TextBlock, new TimeSpan(0, 0, 0, 0, 200), preAnim: (o) => { btnSearch.Text = Application.Current.Resources["search"].ToString(); });
        }



        private void lstBoxHints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Hint data = lstBoxHints.SelectedItem as Hint;
            if (data != null)
            {
                txBoxChoice.Text = data.TownName;
                this.chosenICAO = data.ICAOCode;

                lstBoxHints.Visibility = Visibility.Collapsed;
                this.btnSearch.IsEnabled = true;
            }
        }

        private void txBoxChoice_KeyUp(object sender, KeyEventArgs e)
        {
            string typed = txBoxChoice.Text;
            ObservableCollection<Hint> hints = new AirportLookUp().GetData(choice, typed);

            if (typed.Length > 2)
            {
                lstBoxHints.Visibility = Visibility.Visible;
                lstBoxHints.ItemsSource = hints;
            }

            else
                lstBoxHints.Visibility = Visibility.Collapsed;
        }
    }
}
