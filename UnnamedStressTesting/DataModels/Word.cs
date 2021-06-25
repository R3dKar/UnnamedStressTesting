using System;
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
        /// Включено ли слово в систему тестирования
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Конструктор объекта слова из строки
        /// </summary>
        /// <param name="word">Слово с одной заглавной буквой, характеризующая ударение, и модификаторы с коментариями</param>
        /// <exception cref="ArgumentException"/>
        public Word(string word)
        {
            var parts = word.Split(' ');

            foreach (var part in parts)
            {
                // слОво <+/-> коментарии многословные
                // + - слово включено
                // - - слово отключено
                // после слова коментарии
                switch (part)
                {
                    case "+":
                        Enabled = true;
                        break;
                    case "-":
                        Enabled = false;
                        break;
                    default:
                        if (Letters != null)
                            break;
                        
                        Letters = new List<Letter>(parts[0].Length);

                        foreach (char ch in parts[0])
                        {
                            var letter = new Letter
                            {
                                Character = ch,
                                IsStressed = char.IsUpper(ch)
                            };

                            if (letter.IsStressed && StressIndex == -1)
                                StressIndex = Letters.Count - 1;
                            else if (letter.IsStressed && StressIndex >= 0)
                                throw new ArgumentException("Число ударений в слове больше одного", nameof(word));

                            Letters.Add(letter);
                        }
                        break;
                }
            }

            if (Letters is null)
                throw new ArgumentException("Строка представлена модификаторами без слова", nameof(word));
            if (StressIndex == -1)
                throw new ArgumentException("Слово без ударения", nameof(word));
        }
    }
}
