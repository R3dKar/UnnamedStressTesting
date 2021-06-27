﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Статический класс с методами, облегчающие работу с фаловой системой
    /// </summary>
    public static class FileHelpers
    {
        #region Статические члены

        /// <summary>
        /// Путь до файлов словарей
        /// </summary>
        public static readonly string DictironaryFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Unnamed Stress Testing\Словари");

        /// <summary>
        /// Список всех загруженных словарей на данный момент
        /// </summary>
        public static List<FileDictionary> WordDictionaries = new List<FileDictionary>();

        #endregion

        #region Статические методы

        /// <summary>
        /// Восстанавливает путь до папки хранения словарей
        /// </summary>
        public static void RestoreDictionaryPath()
        {
            if (!Directory.Exists(DictironaryFolderPath))
                Directory.CreateDirectory(DictironaryFolderPath);
        }

        /// <summary>
        /// Полчает список словарей
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDictionaryList()
        {
            RestoreDictionaryPath();
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
            SaveDictionaries();

            WordDictionaries = new List<FileDictionary>();

            GetDictionaryList().ForEach(item => WordDictionaries.Add(new FileDictionary(item)));
        }

        /// <summary>
        /// Обновить словарь для существующих слов
        /// </summary>
        /// <param name="dict">Словарь, который нужно обновить</param>
        public static void UpdateDictionary(FileDictionary dict)
        {
            var lines = GetWords(dict.FilePath);

            string outText = string.Empty;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var w = new Word(line);

                    foreach (var word in dict.Words)
                    {
                        if (word.ToString(false) == w.ToString(false) && word.Comment == w.Comment)
                        {
                            w.Enabled = word.Enabled;
                            break;
                        }
                    }

                    outText += w.ToString(true) + "\n";
                }
                catch (ArgumentException) { }
            }

            File.WriteAllText(dict.FilePath, outText);
        }

        /// <summary>
        /// Сохраняет изменения в словари
        /// </summary>
        public static void SaveDictionaries()
        {
            foreach (var dict in WordDictionaries)
            {
                try
                {
                    UpdateDictionary(dict);
                }
                catch { }
            }
        }

        #endregion
    }
}
