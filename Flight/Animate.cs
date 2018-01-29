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
    class Animation
    {
        /*
            Animate Class - Uses Dependency Properties to Animate different events
            Takes in the Property to be animated, Value of animation, the object to be animated, timespan of the animation and two optionals: method to be perforemd before animation and a method to be performed after the animation 
        */
        public void Animate(DependencyProperty dp, double value, object obj, TimeSpan t, Action<object> preAnim = null, Action<object> postAnim = null)
        {
            if (obj.GetType() == typeof(Ellipse))
            {
                Ellipse temp = obj as Ellipse;
                DoubleAnimation anim = new DoubleAnimation(value, t);
                anim.Completed += new EventHandler(((sender, e) => animation_Complete(sender, postAnim, obj)));
                if (preAnim != null)
                {
                    preAnim.Invoke(temp);
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
                    preAnim.Invoke(temp);
                }
                temp.BeginAnimation(dp, anim);
            }
        }

        //Once animation is complete
        private void animation_Complete(object sender, Action<object> doSomething, object obj)
        {
            if (doSomething != null)
            {
                doSomething.Invoke(obj);
            }
        }
    }
}
