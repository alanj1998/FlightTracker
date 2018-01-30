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

namespace Flight
{
    /// <summary>
    /// Interaction logic for FlightDetails.xaml
    /// </summary>
    public partial class FlightDetails : UserControl
    {
        private DateTime time;
        private string airlineCode;
        private Uri pathToLogo;
        private string town;

        public FlightDetails(DateTime time, string airlineCode, string town, Uri logo = null)
        {
            InitializeComponent();
            this.time = time;
            this.airlineCode = airlineCode;
            this.town = town;

            if (logo == null)
                this.pathToLogo = new Uri("Files/airlineLogo.png", UriKind.Relative);
            else
                this.pathToLogo = logo;

            SetFlightInfo();
        }

        private void SetFlightInfo()
        {
            txBlockAirlineCode.Text = this.airlineCode;
            txBlockTime.Text = $"{this.time.Hour}:{this.time.Minute}";
            txBlockTown.Text = this.town;
            imageLogo.Source = new BitmapImage(this.pathToLogo);
        }
    }
}
