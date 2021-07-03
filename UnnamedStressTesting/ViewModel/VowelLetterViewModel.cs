using System.Windows.Input;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для безударной гласной
    /// </summary>
    public class VowelLetterViewModel : LetterViewModel
    {
        /// <summary>
        /// Команда для неправильного ответа
        /// </summary>
        public ICommand IncorrectAnswerCommand { get; set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="letter">Экземпляр буквы</param>
        public VowelLetterViewModel(Letter letter) : base(letter)
        {
            IncorrectAnswerCommand = new RelayCommand(RevealIncorrectAnswer);
        }

        /// <summary>
        /// Показывает неправильный ответ
        /// </summary>
        private void RevealIncorrectAnswer()
        {
            if (!MainWindowViewModel.MainInstance.IsTestStarted)
                return;

            if (!MainWindowViewModel.MainInstance.IsWordReveal)
            {
                MainWindowViewModel.MainInstance.PressedIndex = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(this);
                MainWindowViewModel.MainInstance.IsWordReveal = true;
            }
        }
    }
}
