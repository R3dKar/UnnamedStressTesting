using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для <see cref="Letter"/>
    /// </summary>
    public class LetterViewModel : BaseViewModel
    {
        /// <summary>
        /// Под ударением буква или нет
        /// </summary>
        public bool IsStressed { get; set; }
        /// <summary>
        /// Символ <see cref="char"/>
        /// </summary>
        public char Character { get; set; }
        /// <summary>
        /// Возвращает <see cref="Character"/>, приведённый к верхнему регистру
        /// </summary>
        public char Uppercase { get => char.ToUpper(Character); }
        /// <summary>
        /// Возвращает <see cref="Character"/>, приведённый к нижнему регистру
        /// </summary>
        public char Lowercase { get => char.ToLower(Character); }
        /// <summary>
        /// Является ли символ гласным или нет
        /// </summary>
        public bool IsVowel { get => Letter.Vowels.Contains(Lowercase); }

        public Color Color { get => IsStressed ? Color.FromRgb(255, 0, 0) : Color.FromRgb(0, 0, 0); }

        /// <summary>
        /// Конструктор из существующего <see cref="Letter"/>
        /// </summary>
        /// <param name="letter">Исходный экземпляр буквы</param>
        public LetterViewModel(Letter letter)
        {
            IsStressed = letter.IsStressed;
            Character = letter.Character;
        }
    }
}
