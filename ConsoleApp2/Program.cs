using System;
using System.Collections.Generic;

class Program
{
    static Dictionary<string, string> DATABASE = new Dictionary<string, string>
    {
        {"Сергей", "Омск"},
        {"Соня", "Москва"},
        {"Алексей", "Калининград"},
        {"Миша", "Москва"},
        {"Дима", "Челябинск"},
        {"Алина", "Красноярск"},
        {"Егор", "Пермь"},
        {"Коля", "Красноярск"},
        {"Артём", "Владивосток"},
        {"Петя", "Михайловка"}
    };

    static Dictionary<string, int> UTC_OFFSET = new Dictionary<string, int>
    {
        {"Москва", 3},
        {"Санкт-Петербург", 3},
        {"Новосибирск", 7},
        {"Екатеринбург", 5},
        {"Нижний Новгород", 3},
        {"Казань", 3},
        {"Челябинск", 5},
        {"Омск", 6},
        {"Самара", 4},
        {"Ростов-на-Дону", 3},
        {"Уфа", 5},
        {"Красноярск", 7},
        {"Воронеж", 3},
        {"Пермь", 5},
        {"Волгоград", 3},
        {"Краснодар", 3},
        {"Калининград", 2},
        {"Владивосток", 10}
    };

    static string FormatCountFriends(int countFriends)
    {
        if (countFriends == 1)
        {
            return "1 друг";
        }
        else if (countFriends >= 2 && countFriends <= 4)
        {
            return $"{countFriends} друга";
        }
        else
        {
            return $"{countFriends} друзей";
        }
    }

    static string WhatTime(string city)
    {
        if (UTC_OFFSET.ContainsKey(city))
        {
            int offset = UTC_OFFSET[city];
            DateTime cityTime = DateTime.UtcNow.AddHours(offset);
            return cityTime.ToString("HH:mm");
        }
        else
        {
            return $"<не могу определить время в городе {city}>";
        }
    }

    static string WhatWeather(string city)
    {
        string url = $"http://wttr.in/{city}";
        try
        {
            using (var client = new System.Net.WebClient())
            {
                string weather = client.DownloadString(url);
                return weather;
            }
        }
        catch (System.Net.WebException)
        {
            return "<сетевая ошибка>";
        }
    }

    static string ProcessAnfisa(string query)
    {
        if (query == "сколько у меня друзей?")
        {
            int count = DATABASE.Count;
            return $"У тебя {FormatCountFriends(count)}.";
        }
        else if (query == "кто все мои друзья?")
        {
            string friendsString = string.Join(", ", DATABASE.Keys);
            return $"Твои друзья: {friendsString}";
        }
        else if (query == "где все мои друзья?")
        {
            var uniqueCities = new HashSet<string>(DATABASE.Values);
            string citiesString = string.Join(", ", uniqueCities);
            return $"Твои друзья в городах: {citiesString}";
        }
        else
        {
            return "<неизвестный запрос>";
        }
    }

    static string ProcessFriend(string name, string query)
    {
        if (DATABASE.ContainsKey(name))
        {
            string city = DATABASE[name];
            if (query == "ты где?")
            {
                return $"{name} в городе {city}";
            }
            else if (query == "который час?")
            {
                string time = WhatTime(city);
                return $"Там сейчас {time}";
            }
            else if (query == "как погода?")
            {
                string weather = WhatWeather(city);
                return weather;
            }
            else
            {
                return "<неизвестный запрос>";
            }
        }
        else
        {
            return $"У тебя нет друга по имени {name}";
        }
    }

    static string ProcessQuery(string query)
    {
        string[] elements = query.Split(new string[] { ", " }, StringSplitOptions.None);
        if (elements[0] == "Анфиса")
        {
            return ProcessAnfisa(elements[1]);
        }
        else
        {
            return ProcessFriend(elements[0], elements[1]);
        }
    }

    static void Runner()
    {
        string[] queries = new string[]
        {
            "Анфиса, сколько у меня друзей?",
            "Анфиса, кто все мои друзья?",
            "Анфиса, где все мои друзья?",
            "Анфиса, кто виноват?",
            "Коля, ты где?",
            "Соня, что делать?",
            "Антон, ты где?",
            "Алексей, который час?",
            "Артём, который час?",
            "Антон, который час?",
            "Петя, который час?",
            "Антон, как погода?"
        };

        foreach (string query in queries)
        {
            Console.WriteLine($"{query} - {ProcessQuery(query)}");
        }
    }

    static void Main()
    {
        Runner();
        Console.ReadLine();
    }
}