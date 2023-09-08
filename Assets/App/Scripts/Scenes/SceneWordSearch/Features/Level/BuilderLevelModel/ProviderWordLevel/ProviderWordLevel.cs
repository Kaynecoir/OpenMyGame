using System;
using System.IO;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string file_text = File.ReadAllText("Asset/App/Resources/Fillwords/" + levelIndex + ".json");
            LevelInfo li = new LevelInfo();
            li.words = new System.Collections.Generic.List<string>(GetStringFromJson(file_text));
            return li;
        }
        static System.Collections.Generic.List<string> GetStringFromJson(string line)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            char c;
            int index = 0;
            bool isWords = false;
            while (index != line.Length - 1)
            {
                c = line[index++];
                if (c == '[') isWords = true;
                if (isWords && c == '\"')
                {
                    c = line[index++];
                    string word = "";
                    while (c != '\"' && c != '\0')
                    {
                        word += c;
                        c = line[index++];
                    }
                    list.Add(word);
                }
            }
            return list;
        }
    }
}