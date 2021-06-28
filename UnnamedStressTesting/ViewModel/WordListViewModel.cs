using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для главного окна
    /// </summary>
    public class WordListViewModel : BaseViewModel
    {
        #region Статиеские члены

        /// <summary>
        /// Главный экземпляр <see cref="WordListViewModel"/>
        /// </summary>
        public static WordListViewModel MainInstance { get; private set; }

        #endregion

        #region Открытые ствойства

        private WordViewModel selectedItem;
        /// <summary>
        /// Выбранный элемент
        /// </summary>
        public WordViewModel SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem == value)
                    return;

                if (value != null && value.IsTreeViewGroup)
                    return;

                ChangeSelectedItem(value, 0.1f);
            }
        }

        /// <summary>
        /// Словори и слова для отображения
        /// </summary>
        public ObservableCollection<WordViewModel> Items { get; set; }

        /// <summary>
        /// Ширина меню выбора слов в пикселях
        /// </summary>
        public int LeftMenuWidth { get; set; } = 200;

        /// <summary>
        /// Контейнер, содержащий левое меню выбора слов
        /// </summary>
        public FrameworkElement LeftMenuContainer { get; set; }

        /// <summary>
        /// Контейнер, содержащий слово справа
        /// </summary>
        public FrameworkElement RightSideWordContainer { get; set; }

        private bool isLeftMenuHidden = false;
        public bool IsLeftMenuHidden
        {
            get => isLeftMenuHidden;
            set
            {
                if (isLeftMenuHidden == value)
                    return;

                isLeftMenuHidden = value;

                if (value)
                    HideLeftMenu(0.4f);
                else
                    ShowLeftMenu(0.4f);
            }
        }
        #endregion

        #region Команды и делегаты

        /// <summary>
        /// Команда открытия папки со словарями
        /// </summary>
        public ICommand OpenDictionaryFolderCommand { get; set; }

        /// <summary>
        /// Команда для перезагрузки словарей
        /// </summary>
        public ICommand RefreshWordCommand { get; set; }

        /// <summary>
        /// Сохраняет изменения в словарях при закрытии приложения
        /// </summary>
        public EventHandler SaveDictionariesOnClose { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public WordListViewModel()
        {
            MainInstance = this;

            OpenDictionaryFolderCommand = new RelayCommand(OpenDictionaryFolder);
            RefreshWordCommand = new RelayCommand(UpdateDictionaries);
            SaveDictionariesOnClose = new EventHandler((s, e) => FileHelpers.SaveDictionaries());

            UpdateDictionaries();
        }

        #endregion

        #region Вспомогательные методы

        /// <summary>
        /// Открывает в проводнике папку со словарями. Если такой папки нет, то путь до них восстанавливается
        /// </summary>
        private void OpenDictionaryFolder()
        {
            FileHelpers.RestoreDictionaryPath();

            Process.Start("explorer", FileHelpers.DictironaryFolderPath);
        }

        /// <summary>
        /// Обновить список словарей и слов
        /// </summary>
        private void UpdateDictionaries()
        {
            FileHelpers.UpdateDictionaries();
            WordViewModel.EnabledWords.Clear();

            SelectedItem = null;
            var items = new ObservableCollection<WordViewModel>();

            foreach (var dict in FileHelpers.WordDictionaries)
            {
                var group = new WordViewModel(dict);
                if (group.Items.Count > 0)
                    items.Add(group);
            }

            Items = items;
        }

        /// <summary>
        /// Анимированно скрывает меню выбора слов
        /// </summary>
        /// <param name="duration">Время анимации в секундах</param>
        private async Task HideLeftMenu(float duration)
        {
            Storyboard storyboard = new Storyboard();

            ThicknessAnimation marginAnimation = new ThicknessAnimation()
            {
                Duration = TimeSpan.FromSeconds(duration),
                From = new Thickness(5),
                To = new Thickness(-LeftMenuWidth, 5, LeftMenuWidth, 5),
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath("Margin"));

            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(duration),
                From = LeftMenuWidth,
                To = 0,
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("Width"));

            storyboard.Children.Add(marginAnimation);
            storyboard.Children.Add(widthAnimation);

            storyboard.Begin(LeftMenuContainer);

            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Анимированно возвращает меню выбора слов
        /// </summary>
        /// <param name="duration">Время анимации в секундах</param>
        private async Task ShowLeftMenu(float duration)
        {
            Storyboard storyboard = new Storyboard();

            ThicknessAnimation marginAnimation = new ThicknessAnimation()
            {
                Duration = TimeSpan.FromSeconds(duration),
                From = new Thickness(-LeftMenuWidth, 5, LeftMenuWidth, 5),
                To = new Thickness(5),
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(marginAnimation, new PropertyPath("Margin"));

            DoubleAnimation widthAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(duration),
                From = 0,
                To = LeftMenuWidth,
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath("Width"));

            storyboard.Children.Add(marginAnimation);
            storyboard.Children.Add(widthAnimation);

            storyboard.Begin(LeftMenuContainer);

            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Анимированно делает слово справа невидимым
        /// </summary>
        /// <param name="duration">Время анимации в секундах</param>
        private async Task FadeOutWord(float duration)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(duration),
                From = 1,
                To = 0,
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            storyboard.Children.Add(opacityAnimation);

            storyboard.Begin(RightSideWordContainer);

            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Анимированно делает слово справа видимым
        /// </summary>
        /// <param name="duration">Время анимации в секундах</param>
        private async Task FadeInWord(float duration)
        {
            Storyboard storyboard = new Storyboard();

            DoubleAnimation opacityAnimation = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(duration),
                From = 0,
                To = 1,
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            storyboard.Children.Add(opacityAnimation);

            storyboard.Begin(RightSideWordContainer);

            await Task.Delay((int)(duration * 1000));
        }

        /// <summary>
        /// Анимированно меняет значение <see cref="WordListViewModel.SelectedItem"/>
        /// </summary>
        /// <param name="value">Устанавливаемое значение</param>
        private async void ChangeSelectedItem(WordViewModel value, float duration)
        {
            await FadeOutWord(duration);
            selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
            await FadeInWord(duration);
        }

        #endregion
    }
}
