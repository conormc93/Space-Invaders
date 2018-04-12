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
using Microsoft.Graphics.Canvas.Text;
using System.Numerics;
using Windows.Devices.Sensors;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPgame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public static CanvasBitmap BG, StartScreen, LevelOne, ScoreScreen, Blast,  EnemyOne, EnemyTwo, SHIP_IMG, MyShip, Boom;
        public static Rect bounds = ApplicationView.GetForCurrentView().VisibleBounds;

        // Timers
        public static DispatcherTimer roundTimer = new DispatcherTimer();
        public static DispatcherTimer enemyTimer = new DispatcherTimer();

        //scaling class can access this info
        //have to make static
        public static float designWidth = 1280;
        public static float designHeight = 720;
        public static float scaleWidth, scaleHeight, pointX, pointY, blastX, blastY, myScore, boomX, boomY, MyShipPOSx, MyShipPOSy;

        //High Score
        public static string STRhighScore;
        public static int highScore;

        public static int boomCount = 60; //testing frame animation
        public static int countdown = 10;
        public static int gameState = 0; // Startscreen

        public static bool roundEnded = false; // when game starts we dont want to trigger this

        //Lists (Projectile)
        public static List<float> blastXPOS = new List<float>();
        public static List<float> blastYPOS = new List<float>();
        public static List<float> blastXPOSs = new List<float>();
        public static List<float> blastYPOSs= new List<float>();
        public static List<float> percent = new List<float>();

        //Lists (Enemies)
        public static List<float> enemyXPOS = new List<float>();
        public static List<float> enemyYPOS = new List<float>();
        public static List<int> enemyShip = new List<int>();
        public static List<String> enemyDirection = new List<String>();

        //Random Generators
        public Random enemyShipType = new Random();
        public Random enemyGenerationInterval = new Random();
        public Random enemyXStart = new Random(); // starting position for ships

        private Inclinometer _inclinometer;
        float roll, pitch, yaw;

        //constructor
        public MainPage()
        {
            this.InitializeComponent();

            //the window is going to bind us to this method
            //when size of window changes fire the event
            Window.Current.SizeChanged += Current_SizeChanged;

            //need to set the scale when the page is being loaded
            Scaling.setScale();

            blastX = (float)bounds.Width / 2;
            blastY = (float)bounds.Height;

            roundTimer.Tick += RoundTimer_Tick;
            roundTimer.Interval = new TimeSpan(0,0,1);

            enemyTimer.Tick += EnemyTimer_Tick;
            //reference the random generator
            enemyTimer.Interval = new TimeSpan(0, 0, 0, 0, enemyGenerationInterval.Next(300, 3000));

            Storage.CreateFile();
            Storage.ReadFile();

            MyShipPOSx = (float)bounds.Width / 2 - (60 * scaleWidth);
            MyShipPOSy = (float)bounds.Height - (137 * scaleHeight);
            //grab the default inclinometer
            _inclinometer = Inclinometer.GetDefault();

            if (_inclinometer != null)
            {
                //establish report interval
                uint minReportInterval = _inclinometer.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _inclinometer.ReportInterval = reportInterval;

                //event handler
                _inclinometer.ReadingChanged += new TypedEventHandler<Inclinometer, InclinometerReadingChangedEventArgs>(ReadingChanged); 
            }
        }

        private async void ReadingChanged(object sender, InclinometerReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                InclinometerReading reading = e.Reading;

                pitch = reading.PitchDegrees;

                if (pitch > 0 && MyShipPOSx < 1100 * scaleWidth)
                {
                    MyShipPOSx += pitch;
                }
                else if (pitch < 0 && MyShipPOSx > 100 * scaleWidth)
                {
                    MyShipPOSx += pitch;
                }
            });
        }

        private void EnemyTimer_Tick(object sender, object e)
        {
            int shipType = enemyShipType.Next(1,3);
            int startingPosition = enemyXStart.Next(0, (int)bounds.Width); // starting position x axis

            if(startingPosition > bounds.Width/2)
            {
                enemyDirection.Add("left");
            }
            else
            {
                enemyDirection.Add("right");
            }

            enemyXPOS.Add(startingPosition);
            enemyYPOS.Add(-50 * scaleHeight);
            enemyShip.Add(shipType);

            enemyTimer.Interval = new TimeSpan(0, 0, 0, 0, enemyGenerationInterval.Next(500, 2000));

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

            /*blastX = (float)bounds.Width / 2;
            blastY = (float)bounds.Height;*/
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
            //images
            StartScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/startscreen.png"));
            LevelOne = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/level-one.png"));
            ScoreScreen = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/scorescreen.png"));
            Blast = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/blast.png"));
            EnemyOne = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/enemy-one.png"));
            EnemyTwo = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/enemy-two.png"));
            MyShip = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/MyShip.png"));
            Boom = await CanvasBitmap.LoadAsync(sender, new Uri("ms-appx:///Assets/images/boom.png"));


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
            
            if (roundEnded == true)
            {
                bool result = Int32.TryParse(STRhighScore, out highScore);
                if (myScore > highScore)
                {
                    Storage.UpdateScore();
                }

                CanvasTextLayout textLayout1 = new CanvasTextLayout(args.DrawingSession, myScore.ToString(), new CanvasTextFormat() { FontFamily= "ms-appx:///Assets/Fonts/pricedown bl.ttf", FontSize = (40 * scaleHeight), WordWrapping = CanvasWordWrapping.NoWrap }, 0.0f, 0.0f);
                args.DrawingSession.DrawTextLayout(textLayout1, ((designWidth * scaleWidth) / 2) - ((float)textLayout1.DrawBounds.Width / 2), 320 * scaleHeight, Colors.White);
                args.DrawingSession.DrawText("High Score: " + highScore, new Vector2(200,200) * scaleWidth, Color.FromArgb(255,200,150,210));

            }
            else
            {
                // Level 1
                if (gameState > 0)
                {

                    args.DrawingSession.DrawText("Score: " + myScore.ToString(), (float)bounds.Width / 2, 10, Color.FromArgb(255, 255, 255, 255));

                    // if we have a point on our x & y axis
                    // that is greater than 0
                    // and our boomCount is greater than 0
                    // then we draw our boom image
                    if (boomX > 0 && boomY > 0 && boomCount > 0)
                    {
                        // to draw the image we get the boom coordinates
                        args.DrawingSession.DrawImage(Scaling.Img(Boom), boomX, boomY);
                        boomCount -= 1;
                    }
                    else
                    {
                        boomCount = 60;
                        boomX = 0;
                        boomY = 0;
                    }



                    //Enemies
                    for (int j = 0; j < enemyXPOS.Count; j++)
                    {
                        if (enemyShip[j] == 1) { SHIP_IMG = EnemyOne; }
                        if (enemyShip[j] == 2) { SHIP_IMG = EnemyTwo; }

                        // instead of hard coding the speed in
                        // possibly change this to difficulty
                        if (enemyDirection[j] == "left")
                        {
                            enemyXPOS[j] -= 4;
                        }
                        else
                        {
                            enemyXPOS[j] += 4;
                        }
                        enemyYPOS[j] += 4;
                        args.DrawingSession.DrawImage(Scaling.Img(SHIP_IMG), enemyXPOS[j], enemyYPOS[j]);
                    }

                    // BLASTS //
                    //Every time we tap on the screen we add 
                    //New blast into the list
                    //Game displays those blasts on the screen
                    for (int i = 0; i < blastXPOS.Count; i++)
                    {
                        //calculate the position of the blast
                        //in betweeen the start position and the clicked position
                        //use linear interpolation formula

                        pointX = (blastXPOSs[i] + (blastXPOS[i] - blastXPOSs[i]) * percent[i]);
                        pointY = (blastYPOSs[i] + (blastYPOS[i] - blastYPOSs[i]) * percent[i]);
                        //pointX = (blastX + (blastXPOS[i] - blastX) * percent[i]);
                        //pointY = (blastY + (blastYPOS[i] - blastY) * percent[i]);

                        args.DrawingSession.DrawImage(Scaling.Img(Blast), pointX - (15 * scaleWidth), pointY - ((float)25 * scaleHeight));

                        percent[i] += (0.050f * scaleHeight);

                        //everytime we move our blasts
                        //we want to check if that blast has 
                        //hit anything
                        for (int h = 0; h < enemyXPOS.Count; h++)
                        {
                            //check the position of the enemies through our points

                            if (pointX >= enemyXPOS[h] && pointX <= enemyXPOS[h] + (185 * scaleWidth) && pointY >= enemyYPOS[h] && pointY <= enemyYPOS[h] + (100 * scaleHeight))
                            {
                                //set the coordinates of the boom
                                boomX = pointX - (90 * scaleWidth);
                                boomY = pointY - (50 * scaleHeight);

                                enemyXPOS.RemoveAt(h);
                                enemyYPOS.RemoveAt(h);
                                enemyShip.RemoveAt(h);
                                enemyDirection.RemoveAt(h);

                                blastXPOS.RemoveAt(i);
                                blastYPOS.RemoveAt(i);
                                blastXPOSs.RemoveAt(i);
                                blastYPOSs.RemoveAt(i);
                                percent.RemoveAt(i);

                                myScore = myScore + 100;

                                break;

                            }//end if
                        }//inner for

                        //remove blasts that go off top of the screen
                        if (pointY < 0f)
                        {
                            blastXPOS.RemoveAt(i);
                            blastYPOS.RemoveAt(i);
                            blastXPOSs.RemoveAt(i);
                            blastYPOSs.RemoveAt(i);
                            percent.RemoveAt(i);
                        }
                    }//end second for

                    args.DrawingSession.DrawImage(Scaling.Img(MyShip), MyShipPOSx, MyShipPOSy);
                    
                }//end inner if

            }//end else

            //redraw everything on the screen
            //redraws each frame i.e 60 FPS
            GameCanvas.Invalidate();
        }

        private void GameCanvas_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (roundEnded == true)
            {

                if ((float)e.GetPosition(GameCanvas).X > 550 * scaleWidth && (float)e.GetPosition(GameCanvas).X < 800 * scaleWidth &&
                    (float)e.GetPosition(GameCanvas).Y > 400 * scaleHeight && (float)e.GetPosition(GameCanvas).Y < 510 * scaleHeight)
                {
                    //show different background
                    gameState = 0;
                    roundEnded = false;
                    countdown = 10;

                    //Stop the enemy timer
                    enemyXPOS.Clear();
                    enemyYPOS.Clear();
                    enemyShip.Clear();
                    enemyDirection.Clear();
                    enemyTimer.Stop();
                    
                }
                
            }
            else
            {
                //if the canvas is tapped
                //change the game state
                if (gameState == 0)
                {
                    gameState += 1;
                    roundTimer.Start();
                    enemyTimer.Start();
                }
                else if (gameState > 0)
                {

                    //we need to add an item to each of our list
                    //depending on where we tap on the game canvas
                    blastXPOSs.Add((float)(MyShipPOSx + (MyShip.Bounds.Width * scaleWidth / 2)));
                    blastYPOSs.Add((float)bounds.Height - (60 * scaleHeight));
                    blastXPOS.Add((float)e.GetPosition(GameCanvas).X);
                    blastYPOS.Add((float)e.GetPosition(GameCanvas).Y);

                    percent.Add(0f);

                }

            }//end else
        }//GameCanvas_Tapped
    }
}
