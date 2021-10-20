using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using System.IO;

namespace spyfall_project
{
    class secondaryMethods
    {
        internal static readonly string locationPath = @"D:\BSUIR\4 sem\Курсовой проект\spyfall\spyfall_project\Файлы\Локации.txt";
        internal static readonly string rulesPath = @"D:\BSUIR\4 sem\Курсовой проект\spyfall\spyfall_project\Файлы\Правила.txt";
        internal static Dictionary<string, int> userNicks = new Dictionary<string, int>();
        internal static int playerCount = 0;

        // ВЫБОР СЛУЧАЙНОЙ ЛОКАЦИИ
        internal static string randomLocation()
        {
            string[] locations = File.ReadAllLines(locationPath);
            Random rand = new Random();
            return locations[rand.Next(locations.Length)];
        }

        // СПИСОК ВСЕХ ЛОКАЦИЙ
        internal static string readTextFile(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                line = sr.ReadToEnd();
                return line;
            }
        }

        // ВЫБОР СЛУЧАЙНОГО ШПИОНА
        internal static string randomSpy()
        {
            Random rand = new Random();
            int rnd = rand.Next( playerCount);
            return userNicks.ElementAt(rnd).Key;
        }
    }
}
