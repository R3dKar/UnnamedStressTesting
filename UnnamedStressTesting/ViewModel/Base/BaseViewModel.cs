using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Класс-шаблон для создания ViewModel. Вызывает PropertyChanged при изменении свойства
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Стандартный PropertyChanged событие для MVVM
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Вызывает событие PropertyChanged вручную
        /// </summary>
        /// <param name="name">Имя свойства</param>
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
