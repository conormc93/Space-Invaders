using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWPgame.Class;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPgame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        CanvasBitmap StartScreen;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;

        //scaling class can access this info
        // have to make static
        public static float DesignWidth = 1280;
        public static float DesignHeight = 720;
        public static float scaleWidth, scaleHeight;



        public MainPage()
        {
            this.InitializeComponent();

            //the window is going to bind us to this method
            //when size of window changes fire the event
            Window.Current.SizeChanged += Current_SizeChanged;

            //need to set the scale when the page is being loaded
            Scaling.setScale();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Scaling.setScale();
        }

        private void GameCanvas_CreateResources(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs args)
        {
            //call task asynchronously
            //won't finish until create resource task is done
            args.TrackAsyncAction(CreateResourcesAsync(sender).AsAsyncAction());
        }

        //keeps thread pool alive so process can happen in background
        async Task CreateResourcesAsync(CanvasControl sender)
        {
            //load startscreen image
            //
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/startscreen.png"));
        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            args.DrawingSession.DrawImage(Scaling.Img(StartScreen));

            //redraw everything on the screen
            //redraws each frame i.e 60 FPS
            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }
    }
}
