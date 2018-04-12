using System;
using Windows.Storage;

namespace UWPgame.Class
{
    class Storage
    {
        //Storage
        public static StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        public static StorageFolder folder = ApplicationData.Current.LocalFolder;

        //Storage Methods
        public static async void CreateFile()
        {
            try
            {
                StorageFolder subFolder = await storageFolder.CreateFolderAsync("Data");
                StorageFile file = await subFolder.CreateFileAsync("MissileCommand.txt", CreationCollisionOption.OpenIfExists);
            }
            catch
            {

            }
        }

        public static async void ReadFile()
        {
            try
            {
                StorageFolder subFolder = await folder.GetFolderAsync("Data");
                StorageFile dataFile = await subFolder.GetFileAsync("MissileCommand.txt");
                MainPage.STRhighScore = await FileIO.ReadTextAsync(dataFile);

            }
            catch
            {

            }
        }

        public static async void UpdateScore()
        {
            bool result = Int32.TryParse(MainPage.STRhighScore, out MainPage.highScore);

            if (MainPage.myScore > MainPage.highScore)
            {
                try
                {
                    StorageFolder subFolder = await folder.GetFolderAsync("Data");
                    StorageFile dataFile = await subFolder.GetFileAsync("MissileCommand.txt");
                    await FileIO.WriteTextAsync(dataFile,MainPage.myScore.ToString());
                    ReadFile();

                }
                catch
                {

                }
            }
            
        }
    }
}
