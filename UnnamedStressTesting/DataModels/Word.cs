using System.Collections.Generic;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Тип для слова
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Список букв слова
        /// </summary>
        public List<Letter> Letters { get; set; }

        /// <summary>
        /// Индекс буквы под ударением
        /// </summary>
        public int StressIndex { get; set; } = -1;

        /// <summary>
        /// Конструктор объекта слова из строки
        /// </summary>
        /// <param name="word">Слово с одной заглавной буквой, характеризующая ударение</param>
        public Word(string word)
        {
            Letters = new List<Letter>(word.Length);
            foreach (char ch in word)
            {
                var letter = new Letter
                {
                    Character = ch,
                    IsStressed = char.IsUpper(ch)
                };

                if (letter.IsStressed)
                    StressIndex = Letters.Count - 1;

                //TODO: добавить проверки на "вшивость" импортируемого слова

                Letters.Add(letter);
            }
        }
    }
}
