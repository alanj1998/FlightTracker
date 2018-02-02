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
using System.Globalization;

namespace FlightTracker
{
    /// <summary>
    /// Interaction logic for FlightsWindow.xaml
    /// </summary>
    public partial class FlightsWindow : Window
    {
        private MainWindow MainMenu;
        private int pageNo = 0;
        private Timer clockTimer = new Timer();
        private List<FlightDetails> departureFlights = new List<FlightDetails>();
        private List<FlightDetails> arrivalFlights = new List<FlightDetails>();
        private const int MAX_NUMBER_PER_PAGE = 7;

        public FlightsWindow(MainWindow main)
        {
            InitializeComponent();
            CultureInfo dateFormat;

            if (UserSettings.Time == "12h")
                dateFormat = new CultureInfo("en-UK");
            else
                dateFormat = new CultureInfo("pl-PL");


            lblTime.Content = DateTime.Now.ToString(dateFormat);
            this.MainMenu = main;

            //Setting up the Timer
            clockTimer.Interval = 1000;
            clockTimer.Elapsed += clockTimer_Elapsed;
            clockTimer.AutoReset = true;
            clockTimer.Start();

            /* Test Data */
            departureFlights = new List<FlightDetails>() {
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Barcelona")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Mad")),
                new FlightDetails(new Flight(DateTime.Now.AddHours(-9), "FR999", "Sligo")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Dublin")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Luton")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Amsterdam")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Warsaw")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Rzeszow")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "London"))
        };
            arrivalFlights = new List<FlightDetails>() {
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Barcelona")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Mad")),
                new FlightDetails(new Flight(DateTime.Now.AddHours(-2), "FR999", "Sligo")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Dublin")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Luton")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Amsterdam")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Warsaw")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "Rzeszow")),
                new FlightDetails(new Flight(DateTime.Now, "FR999", "London"))
        };

            departureFlights.Sort();
            arrivalFlights.Sort();
            SetFlights();
        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            MainMenu.Show();
        }

        private void clockTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Using Dispatcher to update the UI thread
            Dispatcher.Invoke(() =>
            {
                lblTime.Content = DateTime.Now.ToLongTimeString();
            });
        }

        private void changeSlide_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearPlace();
            ChangePage();
            AddFlights(this.departureFlights, Departures);
            AddFlights(this.arrivalFlights, Arrivals);
        }

        private void SetFlights()
        {
            Timer changeSlide = new Timer(10000);
            changeSlide.AutoReset = true;
            changeSlide.Elapsed += changeSlide_Elapsed;
            changeSlide.Start();


            for (int i = 0; i < MAX_NUMBER_PER_PAGE; i++)
            {
                Departures.Children.Add(departureFlights[i]);
                Arrivals.Children.Add(arrivalFlights[i]);
            }
        }

        private void ChangePage()
        {
            double noOfFlights = 0;
            int pages = 0;

            if (this.arrivalFlights.Count > this.departureFlights.Count)
                noOfFlights = this.departureFlights.Count;
            else
                noOfFlights = this.arrivalFlights.Count;

            pages = (int)Math.Floor(noOfFlights / MAX_NUMBER_PER_PAGE);

            if (this.pageNo == pages)
                this.pageNo = 0;
            else
                this.pageNo++;
        }

        private void ClearPlace()
        {
            Dispatcher.Invoke(() =>
            {
                int flightsOnDisplay = Departures.Children.Count;
                for (int i = 0; i < flightsOnDisplay; i++)
                {
                    Departures.Children.RemoveAt(0);
                    Arrivals.Children.RemoveAt(0);
                }
            });
        }

        private void AddFlights(List<FlightDetails> l, StackPanel type)
        {
            int lastFlightIndex = GetLastIndex();
            if (lastFlightIndex > l.Count)
                lastFlightIndex = l.Count;

            Dispatcher.Invoke(() =>
            {
                for (int i = MAX_NUMBER_PER_PAGE * this.pageNo; i < lastFlightIndex; i++)
                    type.Children.Add(l[i]);
            });

        }

        private int GetLastIndex()
        {
            int lastFlightIndex = 0;

            if (this.pageNo == 0)
                lastFlightIndex = MAX_NUMBER_PER_PAGE;
            else
                lastFlightIndex = (this.pageNo + 1) * MAX_NUMBER_PER_PAGE;

            return lastFlightIndex;
        }
    }
}
