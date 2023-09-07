using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            var options1 = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = false
            };
            string file_text = File.ReadAllText("Asset/App/Resources/Fillwords/" + levelIndex + ".json");
            Lib fl = JsonSerializer.Deserialize<Lib>(file_text, options1);
            LevelInfo li = new LevelInfo();
            li.words = new System.Collections.Generic.List<string>(fl.words);
            return li;
        }
        class Lib
        {
            public string[] words { get; set; }
        }
    }
}