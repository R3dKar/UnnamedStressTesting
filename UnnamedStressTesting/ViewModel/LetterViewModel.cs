using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для <see cref="Letter"/>
    /// </summary>
    public class LetterViewModel : BaseViewModel
    {
        #region Открытые свойства

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
        /// Возвращает Character, приведённый к верхнему регистру и преобразованный к типу <see cref="string"/>
        /// </summary>
        public string UppercaseString { get => Uppercase.ToString(); }

        /// <summary>
        /// Возвращает <see cref="Character"/>, приведённый к нижнему регистру
        /// </summary>
        public char Lowercase { get => char.ToLower(Character); }

        /// <summary>
        /// Возвращает Character, приведённый к нижнему регистру и преобразованный к типу <see cref="string"/>
        /// </summary>
        public string LowercaseString { get => Lowercase.ToString(); }

        /// <summary>
        /// Является ли символ гласным или нет
        /// </summary>
        public bool IsVowel { get => Letter.Vowels.Contains(Lowercase); }

        /// <summary>
        /// Цвет буквы
        /// </summary>
        public SolidColorBrush Color { get => IsStressed && !MainWindowViewModel.MainInstance.IsTestStarted ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Black); }
        
        public Cursor Cursor { get => MainWindowViewModel.MainInstance.IsTestStarted && IsVowel ? Cursors.Hand : Cursors.Arrow; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор из существующего <see cref="Letter"/>
        /// </summary>
        /// <param name="letter">Исходный экземпляр буквы</param>
        public LetterViewModel(Letter letter)
        {
            IsStressed = letter.IsStressed;
            Character = letter.Character;
        }

        #endregion
    }
}
