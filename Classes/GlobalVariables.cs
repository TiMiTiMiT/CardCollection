using CardCollection.Classes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardCollection.Classes
{
    public static class GlobalVariables
    {
        public static readonly Dictionary<string, string> Settings = ReadSettingsFile();
        public static readonly string[] GamesAvailiable = ["MTG"];
        public static readonly MTGService MTGService = new MTGService();

        static Dictionary<string, string> ReadSettingsFile()
        {
            Dictionary<string, string> SettingsDictionary = new Dictionary<string, string>();

            try
            {
                StreamReader streamReader = new StreamReader("C:\\Users\\Tim\\source\\repos\\CardCollection\\Savefiles\\Settings\\settings.txt");
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] lineSplit = line.Split(": ");
                    if (lineSplit.Length == 2)
                    {
                        string key = lineSplit[0].Trim();
                        string value = lineSplit[1].Trim();
                        SettingsDictionary[key] = value;
                    }
                }
                streamReader.Close();
            }
            catch (Exception e)
            {
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }

            return SettingsDictionary;
        }

        public static void UpdateSettings()
        {
            using (var writer = new StreamWriter("C:\\Users\\Tim\\source\\repos\\CardCollection\\Savefiles\\Settings\\settings.txt", append: false))
            {
                foreach (string SettingName in Settings.Keys)
                {
                    writer.WriteLine($"{SettingName}: {Settings[SettingName]}");
                }
            }
        }
    }
}
