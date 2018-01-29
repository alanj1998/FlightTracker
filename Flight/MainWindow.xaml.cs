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
using Microsoft.Expression.Drawing;
using System.Windows.Media.Animation;
using System.Threading;
using System.ComponentModel;

namespace Flight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
            Fields necessary to control the mainWindow
            pageNo - the number of the page
            anim - the Animation object used for animating menus
        */
        private int pageNo;
        private Animation anim = new Animation();

        public MainWindow()
        {
            InitializeComponent();

            //Change all outerMenu buttons to opacity of 0
            anim.Animate(Image.OpacityProperty, 0, btnAirportIDCheck, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            anim.Animate(Image.OpacityProperty, 0, btnAirportNameCheck, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            anim.Animate(Image.OpacityProperty, 0, btnAirline, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            anim.Animate(Image.OpacityProperty, 0, btnSettings, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            anim.Animate(Image.OpacityProperty, 0, btnExit, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);

            //Set first page to 0
            this.pageNo = 0;
        }

        #region Animation Methods
        //Changing the Visability of the controls so that it is either displayed or hidden depeding on page number
        private void ChangeVisibility(object o)
        {
            if (o.GetType() == typeof(Image))
            {
                Image i = o as Image;
                if (i.Visibility == Visibility.Visible)
                {
                    i.Visibility = Visibility.Collapsed;
                }
                else
                {
                    i.Visibility = Visibility.Visible;
                }
            }
        }

        //All images were labelled as free to reuse! A dictionary of Image Files
        private Dictionary<string, string> ImagesDict = new Dictionary<string, string>()
        {
            { "Localisation", "Files/localisationImage.png" },
            { "Airport Name", "Files/airportNameLogo.png" },
            { "Airport ID", "Files/airportCodeLogo.png" },
            { "Airline Name", "Files/airlineLogo.png" },
            { "Settings", "Files/settingsLogo.png" },
            { "Exit", "Files/exitLogo.png" }
        };

        //Change the source of the image to change the displayed image
        private void ChangeImageSource(string source)
        {
            Image main = btnUseLocalisation as Image;
            main.Source = new BitmapImage(new Uri(source, UriKind.Relative)); //Changing the image by creating a new bitmapImage from the source
        }        
        #endregion


        #region Event Handlers
        //Main Menu Button
        private void btnMainMenu_Click(object sender, MouseButtonEventArgs e)
        {
            if (this.pageNo == 0)
            {
                anim.Animate(Image.OpacityProperty, 0.5, btnAirportIDCheck, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                anim.Animate(Image.OpacityProperty, 0.5, btnAirportNameCheck, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                this.pageNo++;
            }
            else if (this.pageNo == 1)
            {
                anim.Animate(Image.OpacityProperty, 0, btnAirportIDCheck, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                anim.Animate(Image.OpacityProperty, 0, btnAirportNameCheck, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                anim.Animate(Image.OpacityProperty, 0.5, btnAirline, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                anim.Animate(Image.OpacityProperty, 0.5, btnSettings, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                this.pageNo++;
            }
            else if (this.pageNo == 2)
            {
                anim.Animate(Image.OpacityProperty, 0, btnAirline, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                anim.Animate(Image.OpacityProperty, 0, btnSettings, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                anim.Animate(Image.OpacityProperty, 0.5, btnExit, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                this.pageNo++;
            }
            else
            {
                anim.Animate(Image.OpacityProperty, 0, btnExit, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                this.pageNo = 0;
            }
        }

        //Outer menu buttons
        private void outerBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            Image hovered = sender as Image;
            string name = hovered.Name,
                   message = "",
                   source = "";

            if (name == "btnAirportIDCheck")
            {
                message = "Find Flights by Airport ID";
                source = ImagesDict["Airport ID"];
            }
            else if (name == "btnAirportNameCheck")
            {
                message = "Find Flights by Airport Name";
                source = ImagesDict["Airport Name"];
            }
            else if (name == "btnAirline")
            {
                message = "Find Flights by Airline Name";
                source = ImagesDict["Airline Name"];
            }
            else if (name == "btnSettings")
            {
                message = "Enter Settings";
                source = ImagesDict["Settings"];
            }
            else if(name == "btnExit")
            {
                message = "Exit App";
                source = ImagesDict["Exit"];
            }

            anim.Animate(Image.OpacityProperty, 1, hovered, new TimeSpan(0, 0, 0, 0, 300), preAnim: (o) => { ChangeImageSource(source); });
            lblOptions.Text = message;
        }

        //When the mouse is hovered outside of the button, change opacity back to 50%
        private void outerBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Image hovered = sender as Image;
            anim.Animate(Image.OpacityProperty, 0.5, hovered, new TimeSpan(0, 0, 0, 0, 300));
            lblOptions.Text = "Click For More Options";
            ChangeImageSource(ImagesDict["Localisation"]);
        }

        //Inner localisation button
        private void btnUseLocalisation_MouseEnter(object sender, MouseEventArgs e)
        {
            anim.Animate(Image.OpacityProperty, 1, btnMainMenu, new TimeSpan(0, 0, 0, 0, 300));
            lblOptions.Text = "Click to Find Flights Near You";
        }

        //When the mouse is no longer hovering over localisation button, change the text to click for more options
        private void btnUseLocalisation_MouseLeave(object sender, MouseEventArgs e)
        {
            lblOptions.Text = "Click For More Options";
        }

        //Once the outer button is clicked, determine what button was clicked and do the corresponding action
        private void outerBtn_Click(object sender, MouseButtonEventArgs e)
        {
            Image temp = sender as Image;
            string choice = "";
            switch (temp.Name)
            {
                case "btnAirportNameCheck":
                    choice = "Airport Name";
                    break;
                case "btnAirportIDCheck":
                    choice = "Airport ID";
                    break;
                case "btnAirline":
                    choice = "Airline Name";
                    break;
                case "btnSettings":
                    choice = "Settings";
                    break; 
            }

            if (temp.Name == "btnSettings")
            {
                /*
                SettingsWindow window = new SettingsWindow(this);
                window.Show();
                this.Hide();
                */
            }
            else
            {
                
                SelectWindow w = new SelectWindow(choice, this);
                w.ShowDialog();
            }

            temp.Opacity = 100;
            ChangeImageSource(ImagesDict[choice]);
        }

        //When the Exit button is clicked, terminate the app
        private void btnExit_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        #endregion  
    }
}
