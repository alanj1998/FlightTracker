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

namespace Flight
{
    /// <summary>
    /// Interaction logic for SelectWindow.xaml
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
            lblEnter.Content += this.choice;
        }

        private void btnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void ErrorMessage(bool isEmpty)
        {
            if (!isEmpty)
            {
                MessageBoxResult result = MessageBox.Show($"{txBoxChoice.Text} {this.choice} was not found!\nTry Again?", "Search Error!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show($"{this.choice} can't be empty!\nTry Again!", "Search Error!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txBoxChoice.Text != "")
            {
                this.main.Hide();
                this.Close();
                FlightsWindow flight = new FlightsWindow(main);
                flight.Show();
            }
            else
            {
                ErrorMessage(true);
            } //For testing ErrorMessage()
        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        private void btnSearch_MouseEnter(object sender, MouseEventArgs e)
        {
            Animation.Animate(TextBlock.PaddingProperty, new Thickness(130,3,0,0), sender as TextBlock, new TimeSpan(0, 0, 0, 0, 200), preAnim: (o) => { btnSearch.Text = "...Search"; });
        }

        private void btnSearch_MouseLeave(object sender, MouseEventArgs e)
        {
            Animation.Animate(TextBlock.PaddingProperty, new Thickness(5, 3, 0, 0), sender as TextBlock, new TimeSpan(0, 0, 0, 0, 200), preAnim: (o) => { btnSearch.Text = "Search"; });
        }
    }
}
