﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для <see cref="Word"/>
    /// </summary>
    public class WordViewModel : BaseViewModel
    {
        /// <summary>
        /// Список слов, выбранные в систему тестирования
        /// </summary>
        public static List<WordViewModel> EnabledWords = new List<WordViewModel>();

        /// <summary>
        /// Исходный объект <see cref="UnnamedStressTesting.Word"/>
        /// </summary>
        public Word Word { get; set; }

        /// <summary>
        /// Список букв
        /// </summary>
        public ObservableCollection<LetterViewModel> Letters { get; set; }

        /// <summary>
        /// Индекс буквы под ударением
        /// </summary>
        public int StressIndex { get => Word.StressIndex; }

        /// <summary>
        /// Является ли группой для слов или нет
        /// </summary>
        public bool IsTreeViewGroup { get => Word is null; }

        private ObservableCollection<WordViewModel> items;
        /// <summary>
        /// Возвращает список слов, если <see cref="IsTreeViewGroup"/> равно true
        /// </summary>
        public ObservableCollection<WordViewModel> Items
        {
            get
            {
                if (!IsTreeViewGroup)
                    return null;
                else
                    return items;
            }
            set
            {
                if (items == value)
                    return;

                items = value;
            }
        }

        /// <summary>
        /// Элемет, стоящий по дереву выше
        /// </summary>
        public WordViewModel Parent { get; set; }

        /// <summary>
        /// ViewModel окна
        /// </summary>
        public static WordListViewModel HostViewModel;

        private string preview;
        /// <summary>
        /// Текст для отображения
        /// </summary>
        public string Preview
        {
            get
            {
                if (IsTreeViewGroup)
                    return preview;

                string p = string.Empty;
                foreach (var letter in Letters)
                {
                    p += letter.IsStressed ? letter.Uppercase : letter.Lowercase;
                }

                return p;
            }
            set
            {
                if (!IsTreeViewGroup || preview == value)
                    return;

                preview = value;
            }
        }

        private bool enabled;
        /// <summary>
        /// Включено ли слово в систему проверки или нет
        /// </summary>
        public bool Enabled
        {
            get
            {
                if (IsTreeViewGroup && Items != null)
                {
                    bool isAny = false;
                    foreach (var item in Items)
                    {
                        if (item.Enabled)
                        {
                            isAny = true;
                            break;
                        }
                    }

                    enabled = isAny;
                }

                return enabled;
            }
            set
            {
                if (enabled == value)
                    return;

                enabled = value;

                if (IsTreeViewGroup && Items != null)
                {
                    foreach (var item in Items)
                    {
                        item.Enabled = value;
                    }
                }
                else
                {
                    if (value)
                        EnabledWords.Add(this);
                    else
                        EnabledWords.Remove(this);

                    Word.Enabled = value;

                    Parent?.OnPropertyChanged(nameof(Enabled));
                    OnPropertyChanged(nameof(Enabled));
                }
            }
        }

        /// <summary>
        /// Конструктор для слова
        /// </summary>
        /// <param name="word">Исходный экземпляр слова</param>
        public WordViewModel(Word word)
        {
            Word = word;
            Enabled = word.Enabled;

            if (Enabled)
                EnabledWords.Add(this);

            Letters = new ObservableCollection<LetterViewModel>();

            foreach (var letter in Word.Letters)
            {
                Letters.Add(new LetterViewModel(letter));
            }
        }

        /// <summary>
        /// Конструктор для группы слов
        /// </summary>
        /// <param name="dictionary">Словарь слов</param>
        public WordViewModel(FileDictionary dictionary)
        {
            Preview = Path.GetFileName(dictionary.FilePath).Replace(".txt", "");
            Items = new ObservableCollection<WordViewModel>();

            foreach (var word in dictionary.Words)
            {
                var w = new WordViewModel(word);
                w.Parent = this;
                Items.Add(w);
            }
        }
    }
}
