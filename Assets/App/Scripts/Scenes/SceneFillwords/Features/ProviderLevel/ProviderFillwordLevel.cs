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
			string path_instruct = "Assets\\App\\Resources\\Fillwords\\pack_0.txt";
			string path_words = "Assets\\App\\Resources\\Fillwords\\words_list.txt";
			List<(char, int)> char_list = GetCharInstr(path_instruct, path_words, index);

			int size = Convert.ToInt32(Math.Sqrt(char_list.Count));
			GridFillWords gridFillWords = new GridFillWords(new UnityEngine.Vector2Int(size, size));

			foreach ((char, int) a in char_list)
			{
				gridFillWords.Set(a.Item2 / size, a.Item2 % size, new CharGridModel(a.Item1));
			}

			return gridFillWords;
        }
		List<(char, int)> GetCharInstr(string path_instruct, string path_words, int index)
		{
			List<(char, int)> char_list = new List<(char, int)>();

			string[] line = GetLine(path_instruct, index - 1).Split();
			if(line != null)
			{
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
						return GetCharInstr(path_instruct, path_words, ++index);
					}
				}
			}
			else
			{
				throw new Exception("Wrong in read file of levels.");
			}

			return char_list;
		}
		private string GetLine(string path_to_file, int line)
		{
			if (line < 0) return null;
			using (StreamReader reader_instruct = new StreamReader(path_to_file))
			{
				int yLine = 0;
				string words = null;
				while ((words = reader_instruct.ReadLine()) != null)
				{
					UnityEngine.Debug.Log("line " + line);
					if (yLine == line)
					{
						break;
					}
					yLine++;
				}
				reader_instruct.Close();
				return words;
			}
		}
	}
}