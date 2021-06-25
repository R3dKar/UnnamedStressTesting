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
        /// <summary>
        /// Ширина левого стобца просмотра слов
        /// </summary>
        public int WordListWidth { get; set; } = 200;
        /// <summary>
        /// Ширина левого столбца просмотра слов, преобразованное в <see cref="GridLength"/>
        /// </summary>
        public GridLength WordListWidthGridLength { get => new GridLength(WordListWidth); } //Можно использовать конвертер

        /// <summary>
        /// Команда открытия папки со словарями
        /// </summary>
        public ICommand OpenDictionaryFolderCommand { get; set; }
        /// <summary>
        /// Команда для перезагрузки словарей
        /// </summary>
        public ICommand RefreshWordCommand { get; set; }

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

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public WordListViewModel()
        {
            OpenDictionaryFolderCommand = new RelayCommand(OpenDictionaryFolder);
            RefreshWordCommand = new RelayCommand(UpdateDictionaries);

            UpdateDictionaries();
        }

        /// <summary>
        /// Открывает в проводнике папку со словарями. Если такой папки нет, то путь до них восстанавливается
        /// </summary>
        private void OpenDictionaryFolder()
        {
            if (!Directory.Exists(FileHelpers.DictironaryFolderPath))
                Directory.CreateDirectory(FileHelpers.DictironaryFolderPath);

            Process.Start("explorer", FileHelpers.DictironaryFolderPath);
        }

        /// <summary>
        /// Обновить список словарей и слов
        /// </summary>
        private void UpdateDictionaries()
        {
            FileHelpers.UpdateDictionaries();

            var items = new ObservableCollection<WordViewModel>();
            foreach (var dict in FileHelpers.WordDictionaries)
            {
                var group = new WordViewModel(dict);
                if (group.Items.Count > 0)
                    items.Add(group);
            }

            Items = items;
            SelectedItem = null;
            WordViewModel.EnabledWords.Clear();
        }
    }
}
