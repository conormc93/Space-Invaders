using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;

namespace UWPgame.Class
{
    class Scaling
    {
        
        public static void setScale()
        {
            //Set the bounds of the current view
            MainPage.scaleWidth = (float)MainPage.bounds.Width / MainPage.designWidth;
            MainPage.scaleHeight = (float)MainPage.bounds.Height / MainPage.designHeight;

        }

        //taking the source image we plug into this method
        //use the source image and apply the transform matrix
        //scales the image to the x and y of our width and height
        public static Transform2DEffect Img(CanvasBitmap source)
        {

            Transform2DEffect image;
            image = new Transform2DEffect() { Source = source };
            image.TransformMatrix = Matrix3x2.CreateScale(MainPage.scaleWidth, MainPage.scaleHeight);
            return image;

        }





    }
}
