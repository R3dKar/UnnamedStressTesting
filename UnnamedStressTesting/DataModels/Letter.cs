using System.Collections.Generic;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Тип для буквы
    /// </summary>
    public class Letter
    {
        /// <summary>
        /// Список гласных, приведённые к нижнему регистру
        /// </summary>
        public static readonly List<char> Vowels = new List<char>() { 'а', 'о', 'у', 'ы', 'э', 'и', 'е', 'ю', 'ё', 'я' };
        
        /// <summary>
        /// Символ буквы
        /// </summary>
        public char Character { get; set; }

        /// <summary>
        /// Под ударением или нет
        /// </summary>
        public bool IsStressed { get; set; }

    }
}
