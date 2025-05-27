using CardCollection.Classes.Data;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CardCollection.Classes
{
    public static class GlobalVariables
    {
        static readonly string SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CardCollectionApp");
        public static readonly Dictionary<string, string> Settings = ReadSettingsFile();
        public static readonly string[] GamesAvailiable = ["MTG"];
        public static readonly MTGService MTGService = new MTGService("MTGCardCollection", "MTGDeckCollection");
        // Used to interact with test db
        //public static readonly MTGService MTGService = new MTGService("TestMtgCardCollection", "TestMtgDeckCollection");

        static Dictionary<string, string> ReadSettingsFile()
        {
            string jsonString = File.ReadAllText($"{SettingsPath}\\settings.json");
            Dictionary<string, string>? SettingsDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);

            return SettingsDictionary;
        }

        public static void UpdateSettings()
        {
            string updatedJson = JsonSerializer.Serialize(Settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path.Combine(SettingsPath, "settings.json"), updatedJson);
        }
    }
}
