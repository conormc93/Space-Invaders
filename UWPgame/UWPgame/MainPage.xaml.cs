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
using Windows.UI;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPgame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static CanvasBitmap BG, StartScreen, LevelOne, ScoreScreen, Blast;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;

        //scaling class can access this info
        // have to make static
        public static float designWidth = 1280;
        public static float designHeight = 720;
        public static float scaleWidth, scaleHeight;

        public static int countdown = 3; //testing 3 seconds
        public static bool roundEnded = false; // when game starts we dont want to trigger this

        public static int gameState = 0; // Startscreen

        public static DispatcherTimer roundTimer = new DispatcherTimer();

        //Lists (Projectile)
        public static List<float> blastXPOS = new List<float>();
        public static List<float> blastYPOS = new List<float>();




        //constructor
        public MainPage()
        {
            this.InitializeComponent();

            //the window is going to bind us to this method
            //when size of window changes fire the event
            Window.Current.SizeChanged += Current_SizeChanged;

            //need to set the scale when the page is being loaded
            Scaling.setScale();

            roundTimer.Tick += RoundTimer_Tick;
            roundTimer.Interval = new TimeSpan(0,0,1);
        }

        private void RoundTimer_Tick(object sender, object e)
        {
            countdown -= 1;

            if (countdown < 1)
            {
                roundTimer.Stop();
                roundEnded = true;
            }
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
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/startscreen.png"));
            LevelOne = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/level-one.png"));
            ScoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/scorescreen.png"));
            Blast = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/blast.png"));

        }

        private void GameCanvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            //at the first frame
            //call gsm so game state is at 0
            //load the start screen
            GSM.gameStateManager();

            //dynamically changing the image/state
            args.DrawingSession.DrawImage(Scaling.Img(BG));
            args.DrawingSession.DrawText(countdown.ToString(), 100, 100, Colors.White);


            //Display Blasts
            //Every time we tap on the screen we add 
            //New blast into the list
            //Game displays those blasts on the screen
            for (int i = 0; i < blastXPOS.Count; i++)
            {
                args.DrawingSession.DrawImage(Scaling.Img(Blast), blastXPOS[i], blastYPOS[i]);
            }

            //redraw everything on the screen
            //redraws each frame i.e 60 FPS
            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (roundEnded == true)
            {
                //show different background
                gameState = 0; //testing
                roundEnded = false;
            }
            else
            {
                //if the canvas is tapped
                //change the game state
                if (gameState == 0)
                {
                    gameState += 1;
                    roundTimer.Start();

                    //we need to add an item to each of our list
                    //depending on where we tap on the game canvas
                    blastXPOS.Add((float)e.GetPosition(GameCanvas).X);
                    blastYPOS.Add((float)e.GetPosition(GameCanvas).Y);
                }

            }//end else
        }//GameCanvas_Tapped
    }
}
