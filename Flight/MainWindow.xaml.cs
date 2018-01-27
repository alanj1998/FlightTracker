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
        public MainWindow()
        {
            InitializeComponent();
            Animate(Image.OpacityProperty, 0, btnAirportIDCheck, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            Animate(Image.OpacityProperty, 0, btnAirportNameCheck, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            Animate(Image.OpacityProperty, 0, btnAirline, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
            Animate(Image.OpacityProperty, 0, btnSettings, new TimeSpan(0, 0, 0, 0, 0), ChangeVisibility);
        }

        #region Animation Methods
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

        //All images were labelled as free to reuse!
        private Dictionary<string, string> ImagesDict = new Dictionary<string, string>()
        {
            { "Localisation", "Files/localisationImage.png" },
            { "Airport Name", "Files/airportNameLogo.png" },
            { "Airport ID", "Files/airportCodeLogo.png" },
            { "Airline", "Files/airlineLogo.png" },
            { "Settings", "Files/settingsLogo.png" }
        };

        private void ChangeImageSource(string source)
        {
            Image main = btnUseLocalisation as Image;
            main.Source = new BitmapImage(new Uri(source, UriKind.Relative));
        }

        private void Animate(DependencyProperty dp, double value, object obj, TimeSpan t, Action<object> preAnim = null, Action<object> postAnim = null)
        {
            if (obj.GetType() == typeof(Ellipse))
            {
                Ellipse temp = obj as Ellipse;
                DoubleAnimation anim = new DoubleAnimation(value, t);
                anim.Completed += new EventHandler(((sender, e) => animation_Complete(sender, postAnim, obj)));
                if (preAnim != null)
                {
                    temp.BeginAnimation(dp, anim);
                }
                temp.BeginAnimation(dp, anim);
            }
            else if (obj.GetType() == typeof(Image))
            {
                Image temp = obj as Image;
                DoubleAnimation anim = new DoubleAnimation(value, t);
                anim.Completed += new EventHandler(((sender, e) => animation_Complete(sender, postAnim, obj)));
                if (preAnim != null)
                {
                    preAnim.Invoke(obj);
                }
                temp.BeginAnimation(dp, anim);
            }
        }

        private void animation_Complete(object sender, Action<object> doSomething, object obj)
        {
            if (doSomething != null)
            {
                doSomething.Invoke(obj);
            }
        }
        #endregion


        #region Event Handlers

        //Main Menu Button
        private void btnMainMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (btnAirline.Visibility == Visibility.Collapsed && btnAirportIDCheck.Visibility == Visibility.Collapsed && btnAirportIDCheck.Visibility == Visibility.Collapsed && btnSettings.Visibility == Visibility.Collapsed)
            {
                Animate(Image.OpacityProperty, 0.5, btnAirportIDCheck, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                Animate(Image.OpacityProperty, 0.5, btnAirportNameCheck, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
              //  Animate(Image.OpacityProperty, 0, btnUseLocalisation, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
            }
            else if (btnAirline.Visibility == Visibility.Collapsed && btnSettings.Visibility == Visibility.Collapsed)
            {
                Animate(Image.OpacityProperty, 0, btnAirportIDCheck, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                Animate(Image.OpacityProperty, 0, btnAirportNameCheck, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                Animate(Image.OpacityProperty, 0.5, btnAirline, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
                Animate(Image.OpacityProperty, 0.5, btnSettings, new TimeSpan(0, 0, 0, 0, 150), preAnim: ChangeVisibility);
            }
            else
            {
                Animate(Image.OpacityProperty, 0, btnAirline, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
                Animate(Image.OpacityProperty, 0, btnSettings, new TimeSpan(0, 0, 0, 0, 150), postAnim: ChangeVisibility);
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
                source = ImagesDict["Airline"];
            }
            else if (name == "btnSettings")
            {
                message = "Enter Settings";
                source = ImagesDict["Settings"];
            }

            Animate(Image.OpacityProperty, 1, hovered, new TimeSpan(0, 0, 0, 0, 300), preAnim: (o) => { ChangeImageSource(source); });
            lblOptions.Text = message;
        }

        private void outerBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            Image hovered = sender as Image;
            Animate(Image.OpacityProperty, 0.5, hovered, new TimeSpan(0, 0, 0, 0, 300));
            lblOptions.Text = "Click For More Options";
            ChangeImageSource(ImagesDict["Localisation"]);
        }

        //Inner localisation button
        private void btnUseLocalisation_MouseEnter(object sender, MouseEventArgs e)
        {
            Animate(Image.OpacityProperty, 1, btnMainMenu, new TimeSpan(0, 0, 0, 0, 300));
            lblOptions.Text = "Click to Find Flights Near You";
        }

        private void btnUseLocalisation_MouseLeave(object sender, MouseEventArgs e)
        {
            lblOptions.Text = "Click For More Options";
        }
        #endregion
    }
}
