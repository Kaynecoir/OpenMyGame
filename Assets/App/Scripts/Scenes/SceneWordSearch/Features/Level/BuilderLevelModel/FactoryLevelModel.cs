using System;
using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            List<char> letters = new List<char>();
            for (int i = 0; i < words.Count; i++)
            {
                List<char> addLetters = new List<char>();
                for (int j = 0; j < words[i].Length; j++)
                {
                    if (letters.Contains(words[i][j]))
                    {
                        letters.Remove(words[i][j]);
                    }
                    addLetters.Add(words[i][j]);
                }
                letters.AddRange(addLetters);
                addLetters.Clear();
            }

            return letters;
        }
    }
}