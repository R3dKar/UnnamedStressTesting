using System.Windows.Input;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для ударной гласной
    /// </summary>
    public class StressedLetterViewModel : LetterViewModel
    {
        /// <summary>
        /// Комманда для правильного ответа
        /// </summary>
        public ICommand CorrectAnswerCommand { get; set; }

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        /// <param name="letter">Экземпляр буквы</param>
        public StressedLetterViewModel(Letter letter) : base(letter)
        {
            CorrectAnswerCommand = new RelayCommand(RevealCorrectAnswer);
        }

        /// <summary>
        /// Показывает правильный ответ
        /// </summary>
        private void RevealCorrectAnswer()
        {
            if (!MainWindowViewModel.MainInstance.IsTestStarted)
                return;

            if (!MainWindowViewModel.MainInstance.IsWordReveal)
            {
                MainWindowViewModel.MainInstance.PressedIndex = MainWindowViewModel.MainInstance.SelectedItem.Letters.IndexOf(this);
                MainWindowViewModel.MainInstance.IsWordReveal = true;
            }
            else
            {
                MainWindowViewModel.MainInstance.NextWord();
            }
        }
    }
}
