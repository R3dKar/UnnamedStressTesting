using System;
using System.Collections.Generic;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Словарь из файла
    /// </summary>
    public class FileDictionary
    {
        #region Открытые свойства

        /// <summary>
        /// Путь к файлу словарю
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Список слов словаря
        /// </summary>
        public List<Word> Words { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор словаря
        /// </summary>
        /// <param name="filename">Путь к файлу словаря</param>
        public FileDictionary(string filename)
        {
            FilePath = filename;

            var stringWords = FileHelpers.GetWords(filename);
            Words = new List<Word>();

            foreach (var stringWord in stringWords)
            {
                try
                {
                    Words.Add(new Word(stringWord));
                }
                catch (ArgumentException) { }
            }
        }

        #endregion
    }
}
