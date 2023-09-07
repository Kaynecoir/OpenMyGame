using System;
using System.Collections.Generic;
using System.IO;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        
        
        public GridFillWords LoadModel(int index)
        {
			string path_instruct = "Asset/App/Resources/Fillwords/pack_0.txt";
			string path_words = "Asset/App/Resources/Fillwords/words_list.txt";

			List<(char, int)> char_list = new List<(char, int)>();

			string[] line = GetLine(path_instruct, index).Split();
			for (int i = 0; i < line.Length; i += 2)
			{
				string word = GetLine(path_words, Convert.ToInt32(line[i]));
				string[] word_arr = line[i + 1].Split(";");
				int[] index_arr = new int[word_arr.Length];
				if (word.Length == index_arr.Length)
				{
					for (int j = 0; j < word_arr.Length; j++)
					{
						index_arr[j] = Convert.ToInt32(word_arr[j]);
						char_list.Add((word[j], index_arr[j]));
					}
				}
				else
				{
					return null;
				}
			}
			int size = Convert.ToInt32(Math.Sqrt(char_list.Count));
			GridFillWords gridFillWords = new GridFillWords(new UnityEngine.Vector2Int(size, size));

			foreach ((char, int) a in char_list)
			{
				gridFillWords.Set(a.Item2 / size, a.Item2 % size, new CharGridModel(a.Item1));
			}

			return gridFillWords;
        }
		private string? GetLine(string path_to_file, int line)
		{
			using (StreamReader reader_instruct = new StreamReader(path_to_file))
			{
				int yLine = 0;
				string? words;
				while ((words = reader_instruct.ReadLine()) != null)
				{
					if (yLine == line)
					{
						break;
					}
					yLine++;
				}
				return words;
			}
		}
	}
}