using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPgame.Class
{
    class GSM
    {

        //have 3 game states
        //first: startscreen
        //second: first level
        //third: show hight score
        public static void gameStateManager()
        {
            if (MainPage.roundEnded == true)
            {
                //when the round ends, display the score
                MainPage.BG = MainPage.ScoreScreen;
            }
            else
            {
                if (MainPage.gameState == 0)
                {
                    //the background to our game will be the startscreen
                    MainPage.BG = MainPage.StartScreen;
                }
                else if (MainPage.gameState == 1)
                {
                    MainPage.BG = MainPage.LevelOne;
                }
            }
 
        }

    }
}
