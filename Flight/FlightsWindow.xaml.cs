using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for FlightsWindow.xaml
    /// </summary>
    public partial class FlightsWindow : Window
    {
        private MainWindow MainMenu;
        private Timer timer = new Timer();
        public FlightsWindow(MainWindow main)
        {
            InitializeComponent();

            lblTime.Content = DateTime.Now.ToLongTimeString();
            this.MainMenu = main;

            //Setting up the Timer
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();    
        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            MainMenu.Show();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Using Dispatcher to update the UI thread
            Dispatcher.Invoke(() =>
            {
                lblTime.Content = DateTime.Now.ToLongTimeString();
            });  
        }
    }
}
