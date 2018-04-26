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

        public FlightsWindow(string airportCode, string airlineCode = null)
        {
            InitializeComponent();

            //If the usersettings contain 12h - set the time to AM/PM else set it to 24H system
            if (UserSettings.Time == "12h")
                dateFormat = new CultureInfo("en-UK");
            else
                dateFormat = new CultureInfo("pl-PL");

            //Set the time to the label
            lblTime.Content = DateTime.Now.ToString(dateFormat);

            //Setting up the Timer
            clockTimer.Interval = 1000;
            clockTimer.Elapsed += clockTimer_Elapsed;
            clockTimer.AutoReset = true;
            clockTimer.Start();

            //Load flight data
            data = new LoadFlightData(airportCode, airlineCode);

            //Set flights
            this.isSet = SetFlights();
            if (this.isSet)
                SetAirportInfo();
        }

        //Upon destruction delete all timers
        ~FlightsWindow()
        {
            this.clockTimer.Dispose();
            this.changeSlide.Dispose();
        }

        private void btnClose_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void clockTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Using Dispatcher to update the UI thread
            try
            {
                Dispatcher.Invoke(() =>
                {
                    lblTime.Content = DateTime.Now.ToString(dateFormat);
                });
            }
            catch (Exception ex) { }

        }

        //When there is time to change the slide, clear all flights and add next 7 flights to the page
        private void changeSlide_Elapsed(object sender, ElapsedEventArgs e)
        {
            ClearPlace();
            ChangePage();
            AddFlights(this.departureFlights, Departures);
            AddFlights(this.arrivalFlights, Arrivals);
        }

        //Set flights to the Collection of flights
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

        //Set info of the airport
        private void SetAirportInfo()
        {
            txBlockAirportID.Text = data.AirportInfo.AirportCode;
            txBlockAirportLocation.Text = data.AirportInfo.AirportLocation;
            txBlockAirportName.Text = data.AirportInfo.AirportName;
        }

        //Method used to change the flight
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

        //Method used to clear out the StackPanel of events
        private void ClearPlace()
        {
            Dispatcher.Invoke(() =>
            {
                this.Departures.Children.Clear();
                this.Arrivals.Children.Clear();
            });
        }

        //Add Flights to the stackpanel
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

        //Used to get the index of the flight object
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

        //Displaying error messages
        private void ErrorMessage()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show(Application.Current.Resources["errorNoData"].ToString(), Application.Current.Resources["errorTitle"].ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
