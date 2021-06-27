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

        /// <summary>
        /// Ширина левого стобца просмотра слов
        /// </summary>
        public int WordListWidth { get; set; } = 200;

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

                selectedItem = value;
            }
        }

        /// <summary>
        /// Словори и слова для отображения
        /// </summary>
        public ObservableCollection<WordViewModel> Items { get; set; }

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

        #endregion
    }
}
