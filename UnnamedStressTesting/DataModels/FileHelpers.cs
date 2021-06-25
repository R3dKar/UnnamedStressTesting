using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Статический класс с методами, облегчающие работу с фаловой системой
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// Путь до файлов словарей
        /// </summary>
        public static readonly string DictironaryFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Unnamed Stress Testing\Словари");
        
        /// <summary>
        /// Список всех загруженных словарей на данный момент
        /// </summary>
        public static List<FileDictionary> WordDictionaries;

        /// <summary>
        /// Полчает список словарей
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDictionaryList()
        {
            return new List<string>(Directory.GetFiles(DictironaryFolderPath, "*.txt"));
        }

        /// <summary>
        /// Получает список слов из файла
        /// </summary>
        /// <param name="dictionary">Путь к файлу</param>
        /// <returns></returns>
        public static List<string> GetWords(string dictionary)
        {
            string path = Path.Combine(DictironaryFolderPath, dictionary);

            return File.ReadAllText(path).Split(new char[] { '\n' })
                .Select(item => Regex.Replace(item.Replace("\r", ""), @"\s+", " "))
                .Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
        }

        /// <summary>
        /// Обновляет список словарей и перезагружает их
        /// </summary>
        public static void UpdateDictionaries()
        {
            WordDictionaries = new List<FileDictionary>();

            GetDictionaryList().ForEach(item => WordDictionaries.Add(new FileDictionary(item)));
        }
    }
}
