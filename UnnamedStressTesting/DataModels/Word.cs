using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Тип для слова
    /// </summary>
    public class Word
    {
        #region Статические члены
        
        /// <summary>
        /// Regex выражение для парсинга слова из строки
        /// </summary>
        public static readonly Regex WordPattern = new Regex(@"(?<precontext>[а-я\s]*\s)?(?<word>[а-я]*[АОУЫЭИЕЁЮЯ][а-я]*)(?<postcontext>\s[а-я\s]*)?(?<include>[+-])?(\s*(;|//|#)?\s*(?<comment>\S.*))?");

        #endregion

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

        /// <summary>
        /// Контекст перед проверяемым словом
        /// </summary>
        public string PreContext { get; set; }
        /// <summary>
        /// Контекст после проверяемого слова
        /// </summary>
        public string PostContext { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор объекта слова из строки
        /// </summary>
        /// <param name="word">Слово с одной заглавной буквой, характеризующая ударение, и модификаторы с коментариями</param>
        /// <exception cref="ArgumentException"/>
        public Word(string word)
        {
            // контекст слОво контекст [+/-] ;коментарии многословные
            // + - слово включено
            // - - слово отключено
            // после ';', '//', '#' коментарий к слову

            Match parsed = WordPattern.Match(word);

            if (parsed.Success)
            {
                if (parsed.Groups["precontext"].Success && !string.IsNullOrWhiteSpace(parsed.Groups["precontext"].Value))
                    PreContext = Regex.Replace(parsed.Groups["precontext"].Value.TrimStart(), @"\s+", " ");

                if (parsed.Groups["postcontext"].Success && !string.IsNullOrWhiteSpace(parsed.Groups["postcontext"].Value))
                    PostContext = Regex.Replace(parsed.Groups["postcontext"].Value.TrimEnd(), @"\s+", " ");

                if (parsed.Groups["include"].Success)
                    Enabled = parsed.Groups["include"].Value == "+" ? true : false;

                if (parsed.Groups["comment"].Success)
                    Comment = parsed.Groups["comment"].Value.TrimEnd();

                Letters = new List<Letter>(parsed.Groups["word"].Value.Length);
                foreach (char ch in parsed.Groups["word"].Value)
                {
                    Letter letter = new Letter()
                    {
                        Character = ch,
                        IsStressed = char.IsUpper(ch)
                    };

                    if (letter.IsStressed)
                        StressIndex = Letters.Count;

                    Letters.Add(letter);
                }
            }
            else
                throw new ArgumentException("Ударений нет или их несколько", nameof(word));
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
            string word = PreContext;

            foreach (var letter in Letters)
            {
                word += letter.IsStressed ? char.ToUpper(letter.Character) : char.ToLower(letter.Character);
            }

            word += PostContext;

            if (serialize)
            {
                word += " " + (Enabled ? "+" : "-");

                if (Comment != null)
                    word += $" ;{Comment}";
            }

            return word;
        }

        #endregion
    }
}
