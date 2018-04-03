using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace UWPgame.Class
{
    class Scaling
    {
        
        public void setScale()
        {
            //Set the bounds of the current view
            Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            MainPage.scaleWidth = (float)bounds.Width / MainPage.DesignWidth;
            MainPage.scaleHeight = (float)bounds.Height / MainPage.DesignHeight;

        }

        //public Transform2DEffect img(CanvasBitmap source)
        //{



        //}





    }
}
