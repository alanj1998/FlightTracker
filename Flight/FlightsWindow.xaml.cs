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
        private Timer changeSlide = new Timer(10000);
        private List<FlightDetails> departureFlights = new List<FlightDetails>();
        private List<FlightDetails> arrivalFlights = new List<FlightDetails>();
        private CultureInfo dateFormat;
        private LoadFlightData data;
        private bool isSet = false;

        public bool IsSet
        {
            get
            {
                return this.isSet;
            }
        }

        public FlightsWindow(MainWindow main, string airportCode, string airlineCode = null)
        {
            InitializeComponent();

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

            data = new LoadFlightData(airportCode, airlineCode);

            /* Test Data
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
        */
            this.isSet = SetFlights();
            if (this.isSet)
                SetAirportInfo();
        }

        ~FlightsWindow()
        {
            this.clockTimer.Dispose();
            this.changeSlide.Dispose();
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
                lblTime.Content = DateTime.Now.ToString(dateFormat);
            });
        }

        private void changeSlide_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearPlace();
            ChangePage();
            AddFlights(this.departureFlights, Departures);
            AddFlights(this.arrivalFlights, Arrivals);
        }

        private bool SetFlights()
        {
            departureFlights = data.Departures;
            arrivalFlights = data.Arrivals;

            if (departureFlights.Count > 0 && arrivalFlights.Count > 0)
            {
                const int MAX_NUMBER_PER_PAGE = 7;
                changeSlide.AutoReset = true;
                changeSlide.Elapsed += changeSlide_Elapsed;

                this.departureFlights.Sort();
                this.arrivalFlights.Sort();

                ClearPlace();

                for (int i = 0; i < MAX_NUMBER_PER_PAGE; i++)
                {
                    this.Departures.Children.Add(departureFlights[i]);
                    this.Arrivals.Children.Add(arrivalFlights[i]);
                }

                changeSlide.Start();
                return true;
            }
            else
            {
                this.ErrorMessage();
                return false;
            }
        }

        private void SetAirportInfo()
        {
            txBlockAirportID.Text = data.AirportInfo.AirportCode;
            txBlockAirportLocation.Text = data.AirportInfo.AirportLocation;
            txBlockAirportName.Text = data.AirportInfo.AirportName;
        }

        private void ChangePage()
        {
            double noOfFlights = 0;
            int pages = 0;
            const int MAX_NUMBER_PER_PAGE = 7;

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
                this.Departures.Children.Clear();
                this.Arrivals.Children.Clear();
            });
        }

        private void AddFlights(List<FlightDetails> l, StackPanel type)
        {
            const int MAX_NUMBER_PER_PAGE = 7;
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
            const int MAX_NUMBER_PER_PAGE = 7;
            int lastFlightIndex = 0;

            if (this.pageNo == 0)
                lastFlightIndex = MAX_NUMBER_PER_PAGE;
            else
                lastFlightIndex = (this.pageNo + 1) * MAX_NUMBER_PER_PAGE;

            return lastFlightIndex;
        }

        private void ErrorMessage()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(Application.Current.Resources["errorNoData"].ToString(), Application.Current.Resources["errorTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
