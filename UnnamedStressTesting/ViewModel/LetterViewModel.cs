namespace UnnamedStressTesting
{
    /// <summary>
    /// Абстрактный класс ViewModel для <see cref="Letter"/>
    /// </summary>
    public abstract class LetterViewModel : BaseViewModel
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

        /// <summary>
        /// Исходя из передаваемого <see cref="Letter"/> возвращает ViewModel наследуемого от <see cref="LetterViewModel"/>
        /// </summary>
        /// <param name="letter">Буква</param>
        /// <returns>ViewModel нужного типа</returns>
        public static LetterViewModel GetViewModel(Letter letter)
        {
            if (!letter.IsVowel)
                return new ConsonantLetterViewModel(letter);
            else if (letter.IsStressed)
                return new StressedLetterViewModel(letter);
            else
                return new VowelLetterViewModel(letter);
        }

        #endregion
    }
}
