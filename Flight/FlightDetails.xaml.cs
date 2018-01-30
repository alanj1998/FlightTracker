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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightTracker
{
    /// <summary>
    /// Interaction logic for FlightDetails.xaml
    /// </summary>
    public partial class FlightDetails : UserControl, IComparable
    {
        private Flight flight; 

        public FlightDetails(Flight flight)
        {
            InitializeComponent();

            this.flight = flight;
            SetFlightInfo();
        }

        public int CompareTo(object obj)
        {
            FlightDetails temp = obj as FlightDetails;
            return this.flight.Time.CompareTo(temp.flight.Time);
        }

        //Set up the view of the flight
        private void SetFlightInfo()
        {
            txBlockAirlineCode.Text = this.flight.AirlineCode;
            txBlockTime.Text = $"{this.flight.Time.Hour}:{this.flight.Time.Minute}";
            txBlockTown.Text = this.flight.Town;
            imageLogo.Source = new BitmapImage(this.flight.PathToLogo);
        }
    }
}
