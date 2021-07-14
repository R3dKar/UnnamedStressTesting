using System.Windows.Input;

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

        #endregion

        #region Команды

        /// <summary>
        /// Команды выбора буквы
        /// </summary>
        public ICommand LetterCommand { get; set; }

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

            LetterCommand = new RelayCommand(RevealWord);
        }

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Раскрывает слово
        /// </summary>
        private void RevealWord()
        {
            if (!MainWindowViewModel.MainInstance.IsTestStarted || (MainWindowViewModel.MainInstance.IsWordReveal && !IsStressed) || !IsVowel)
                return;

            if (MainWindowViewModel.MainInstance.IsWordReveal && IsStressed)
            {
                MainWindowViewModel.MainInstance.NextWord();

                //TODO: добавить логику для следующего слова
                
                return;
            }

            MainWindowViewModel.MainInstance.IsWordReveal = true;
            MainWindowViewModel.MainInstance.PressedIndex = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(this);
            MainWindowViewModel.MainInstance.StartShowingHint();

            if (IsStressed)
            {
                //TODO: добавить логику для правильного ответа
            }
            else
            {
                //TODO: доюавить логику для неправильного ответа
            }
        }

        #endregion
    }
}
