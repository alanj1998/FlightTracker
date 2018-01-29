using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Flight
{
    static class Animation
    {
        /*
            Animate Class - Uses Dependency Properties to Animate different events
            Takes in the Property to be animated, Value of animation, the object to be animated, timespan of the animation and two optionals: method to be perforemd before animation and a method to be performed after the animation 
        */

        //Animating an Ellipse with a double based dependence
        static public void Animate(DependencyProperty dp, double value, Ellipse obj, TimeSpan t, Action<object> preAnim = null, Action<object> postAnim = null)
        {
            DoubleAnimation anim = new DoubleAnimation(value, t);
            anim.Completed += new EventHandler(((sender, e) => animation_Complete(sender, postAnim, obj)));
            if (preAnim != null)
            {
                preAnim.Invoke(obj);
            }
            obj.BeginAnimation(dp, anim);
        }

        //Animating an image with a double based dependency
        static public void Animate(DependencyProperty dp, double value, Image obj, TimeSpan t, Action<object> preAnim = null, Action<object> postAnim = null)
        {
            DoubleAnimation anim = new DoubleAnimation(value, t);
            anim.Completed += new EventHandler(((sender, e) => animation_Complete(sender, postAnim, obj)));
            if (preAnim != null)
            {
                preAnim.Invoke(obj);
            }
            obj.BeginAnimation(dp, anim);
        }

        //Animating a TextBlock with a thickness dependency
        static public void Animate(DependencyProperty dp, Thickness thickness, TextBlock obj, TimeSpan t, Action<object> preAnim = null, Action<object> postAnim = null)
        {
            ThicknessAnimation anim = new ThicknessAnimation(thickness, t);
            anim.Completed += new EventHandler(((sender, e) => animation_Complete(sender, postAnim, obj)));
            if (preAnim != null)
            {
                preAnim.Invoke(obj);
            }
            obj.BeginAnimation(dp, anim);
        }

        //Once animation is complete
        static private void animation_Complete(object sender, Action<object> doSomething, object obj)
        {
            if (doSomething != null)
            {
                doSomething.Invoke(obj);
            }
        }
    }
}
