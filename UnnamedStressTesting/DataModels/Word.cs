using System;
using System.Collections.Generic;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Тип для слова
    /// </summary>
    public class Word
    {
        #region Открытые свойства

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
        /// Коментарий к слову
        /// </summary>
        public string Comment { get; set; } = null;

        #endregion

        #region Конструкторы

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
                        if (!string.IsNullOrEmpty(Comment))
                            Comment += " +";
                        else
                            Enabled = true;
                        break;
                    case "-":
                        if (!string.IsNullOrEmpty(Comment))
                            Comment += " -";
                        else
                            Enabled = false;
                        break;
                    default:
                        if (Letters != null)
                        {
                            if (Comment is null)
                                Comment = part;
                            else
                                Comment += " " + part;
                            break;
                        }

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

        #endregion
        
        #region Вспомогательные методы

        /// <summary>
        /// Возвращает строку, характеризующую экземпляр класса
        /// </summary>
        /// <param name="serialize">Если true, то выдаёт строку готовую к записи в файл</param>
        /// <returns>Строка типа <see cref="string"/></returns>
        public string ToString(bool serialize)
        {
            string word = string.Empty;

            foreach (var letter in Letters)
            {
                word += letter.Character;
            }

            if (serialize)
            {
                word += " " + (Enabled ? "+" : "-");

                if (Comment != null)
                    word += ' ' + Comment;
            }

            return word;
        }

        #endregion
    }
}
