using System.Collections.Generic;
using System.Linq;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Словарь из файла
    /// </summary>
    public class FileDictionary
    {
        /// <summary>
        /// Путь к файлу словарю
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Список слов словаря
        /// </summary>
        public List<Word> Words { get; set; }

        /// <summary>
        /// Конструктор словаря
        /// </summary>
        /// <param name="filename">Путь к файлу словаря</param>
        public FileDictionary(string filename)
        {
            FilePath = filename;
            Words = FileHelpers.GetWords(FilePath).Select(item => new Word(item)).ToList();
        }
    }
}
