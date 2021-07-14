using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using System.Collections.Specialized;
using System.Threading;
using System.IO;
using System.Net;
using System.Collections.Generic;
using TaskDialogInterop;

namespace UnnamedStressTesting
{
    /// <summary>
    /// ViewModel для главного окна
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Статиеские члены

        /// <summary>
        /// Главный экземпляр <see cref="MainWindowViewModel"/>
        /// </summary>
        public static MainWindowViewModel MainInstance { get; private set; }

        /// <summary>
        /// Список слов, выбранные в систему тестирования
        /// </summary>
        public static ObservableCollection<WordViewModel> EnabledWords { get; private set; } = new ObservableCollection<WordViewModel>();

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
                if (selectedItem == value && !IsTestStarted)
                    return;

                if (value != null && value.IsTreeViewGroup)
                    return;

                ChangeSelectedItem(value, 0.1f);
            }
        }

        private ObservableCollection<WordViewModel> items;
        /// <summary>
        /// Словори и слова для отображения
        /// </summary>
        public ObservableCollection<WordViewModel> Items
        {
            get => items;
            set
            {
                if (items == value)
                    return;

                items = value;
                OnPropertyChanged(nameof(IsItemsEmpty));
            }
        }

        /// <summary>
        /// Показывает, пуст ли список слов
        /// </summary>
        public bool IsItemsEmpty
        {
            get
            {
                if (Items == null)
                    return true;
                else
                    return Items.Count == 0;
            }
        }

        /// <summary>
        /// Ширина меню выбора слов в пикселях
        /// </summary>
        public int LeftMenuWidth { get; set; } = 200;

        /// <summary>
        /// Контейнер, содержащий левое меню выбора слов
        /// </summary>
        public FrameworkElement LeftMenuContainer { get; set; }

        /// <summary>
        /// Показывает, обновляется ли список слов
        /// </summary>
        public bool IsWordRefresh { get; set; }

        /// <summary>
        /// Контейнер, содержащий слово справа
        /// </summary>
        public FrameworkElement RightSideWordContainer { get; set; }

        private bool isLeftMenuHidden = false;
        /// <summary>
        /// Показывает, скрыто ли левое меню или нет
        /// </summary>
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

        /// <summary>
        /// Показывает, запущен ли тест или нет
        /// </summary>
        public bool IsTestStarted { get; set; }

        /// <summary>
        /// Показывает, пуст ли список слов <see cref="MainWindowViewModel.EnabledWords"/>
        /// </summary>
        public bool IsEnabledWordEmpty { get => EnabledWords?.Count == 0; }

        /// <summary>
        /// Показывает правильный ответ для тестирования или нет
        /// </summary>
        public bool IsWordReveal { get; set; }

        /// <summary>
        /// Показывает индекс выбранной в тесте буквы
        /// </summary>
        public int PressedIndex { get; set; }

        /// <summary>
        /// Источник токена для отмены задачи на показ подсказки
        /// </summary>
        private CancellationTokenSource hintTokenSource;
        /// <summary>
        /// Получает токен для отмены задачи
        /// </summary>
        public CancellationToken HintCancellationToken { get => hintTokenSource.Token; }

        /// <summary>
        /// Показывает, показывается ли подсказка или нет
        /// </summary>
        public bool ShowNextQuestionHint { get; set; } = false;

        /// <summary>
        /// Источник токена для отмены скачивания
        /// </summary>
        private CancellationTokenSource downloadTokenSource;
        /// <summary>
        /// Токен для отмены скачивания
        /// </summary>
        public CancellationToken DownloadCancellationToken { get => downloadTokenSource.Token; }

        /// <summary>
        /// Показывает, загружаются ли сейчас файлы
        /// </summary>
        public bool IsDownloading { get; set; }

        /// <summary>
        /// Текст для подсказки на кнопке загрузки
        /// </summary>
        public string DownloadingToolTipInfo { get; set; }

        /// <summary>
        /// Показывает количество процентов
        /// </summary>
        public int DownloadedPercent { get; set; }

        /// <summary>
        /// Словарь файл - сколько скачано
        /// </summary>
        public Dictionary<string, long> DownloadedBytes { get; set; }
        /// <summary>
        /// Словарь файл - размер
        /// </summary>
        public Dictionary<string, long> BytesToDownload { get; set; }

        /// <summary>
        /// Показывает способ разрешения конфликта файлов
        /// </summary>
        public DownloadPermissionType PermissionType { get; set; } = DownloadPermissionType.None;

        /// <summary>
        /// Конфиг для диологового окна запроса разрешения
        /// </summary>
        public TaskDialogOptions PermissionTaskDialogOptions;

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
        /// Команда для остановки тестирования
        /// </summary>
        public ICommand StartTestingCommand { get; set; }

        /// <summary>
        /// Команда для остановки тестирования
        /// </summary>
        public ICommand StopTestingCommand { get; set; }

        /// <summary>
        /// Команда для остановки тестирования
        /// </summary>
        public ICommand DownloadDictionariesCommand { get; set; }

        /// <summary>
        /// Сохраняет изменения в словарях при закрытии приложения
        /// </summary>
        public EventHandler SaveDictionariesOnClose { get; set; }

        /// <summary>
        /// Вызывает <see cref="BaseViewModel.OnPropertyChanged"/> при изменении коллекции <see cref="MainWindowViewModel.EnabledWords"/>
        /// </summary>
        public NotifyCollectionChangedEventHandler UpdateOnEnabledWordsChanged { get; set; }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Стандартный конструктор
        /// </summary>
        public MainWindowViewModel()
        {
            MainInstance = this;

            OpenDictionaryFolderCommand = new RelayCommand(OpenDictionaryFolder);
            RefreshWordCommand = new RelayCommand(UpdateDictionaries);
            StartTestingCommand = new RelayCommand(StartTesting);
            StopTestingCommand = new RelayCommand(StopTesting);
            DownloadDictionariesCommand = new ParametrizedRelayCommand(DownloadDictionaries);

            SaveDictionariesOnClose = new EventHandler((s, e) => FileHelpers.SaveDictionaries());
            UpdateOnEnabledWordsChanged = new NotifyCollectionChangedEventHandler((s, e) => OnPropertyChanged(nameof(IsEnabledWordEmpty)));
            EnabledWords.CollectionChanged += UpdateOnEnabledWordsChanged;

            hintTokenSource = new CancellationTokenSource();
            downloadTokenSource = new CancellationTokenSource();

            DownloadingToolTipInfo = "Скачать словари";

            DownloadedBytes = new Dictionary<string, long>();
            BytesToDownload = new Dictionary<string, long>();

            PermissionTaskDialogOptions = new TaskDialogOptions();
            PermissionTaskDialogOptions.Title = "Конфликт";
            PermissionTaskDialogOptions.MainInstruction = "Обнаружен конфликт файлов словерей";
            PermissionTaskDialogOptions.Content = "Выберете необходимое действие";
            PermissionTaskDialogOptions.CustomButtons = new string[] { "Перезаписать", "Переименовать", "Отмена" };
            PermissionTaskDialogOptions.DefaultButtonIndex = 2;
            PermissionTaskDialogOptions.AllowDialogCancellation = true;
            PermissionTaskDialogOptions.MainIcon = VistaTaskDialogIcon.Warning;
            PermissionTaskDialogOptions.FooterText = "Переименовывание произойдёт автоматически";
            PermissionTaskDialogOptions.FooterIcon = VistaTaskDialogIcon.Information;

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
        private async void UpdateDictionaries()
        {
            if (IsWordRefresh)
                return;

            IsWordRefresh = true;
            SelectedItem = null;

            await Task.Run(() =>
            {
                FileHelpers.UpdateDictionaries();
                EnabledWords.Clear();
                
                var items = new ObservableCollection<WordViewModel>();

                foreach (var dict in FileHelpers.WordDictionaries)
                {
                    var group = new WordViewModel(dict);
                    if (group.Items.Count > 0)
                        items.Add(group);
                }

                Items = items;
            });

            IsWordRefresh = false;
        }

        /// <summary>
        /// Запускает тестирование
        /// </summary>
        private void StartTesting()
        {
            IsTestStarted = true;
            IsLeftMenuHidden = true;
            IsWordReveal = false;
            PressedIndex = -1;
            SelectedItem = EnabledWords[FileHelpers.random.Next(EnabledWords.Count)];
        }

        /// <summary>
        /// Останавливает тестирование
        /// </summary>
        private void StopTesting()
        {
            hintTokenSource.Cancel();
            hintTokenSource.Dispose();
            hintTokenSource = new CancellationTokenSource();

            IsTestStarted = false;
            IsLeftMenuHidden = false;
            IsWordReveal = false;
            PressedIndex = -1;
            ShowNextQuestionHint = false;
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
        /// Анимированно меняет значение <see cref="MainWindowViewModel.SelectedItem"/>
        /// </summary>
        /// <param name="value">Устанавливаемое значение</param>
        private async void ChangeSelectedItem(WordViewModel value, float duration)
        {
            await FadeOutWord(duration);
            selectedItem = value;
            IsWordReveal = false;
            PressedIndex = -1;
            OnPropertyChanged(nameof(SelectedItem));
            await FadeInWord(duration);
        }

        /// <summary>
        /// Запускает таймер на показ подсказки для перехода к следующему вопросу (система просто не очень очевидная)
        /// </summary>
        public async void StartShowingHint()
        {
            CancellationToken token = HintCancellationToken;

            await Task.Delay(4000);
            if (token.IsCancellationRequested)
            {
                return;
            }
            ShowNextQuestionHint = true;
        }

        /// <summary>
        /// Меняет текущее слово на следующее случайным образом
        /// </summary>
        public void NextWord()
        {
            if (!IsTestStarted)
                return;

            var word = EnabledWords[FileHelpers.random.Next(EnabledWords.Count)];

            while (word == SelectedItem && EnabledWords.Count > 1)
                word = EnabledWords[FileHelpers.random.Next(EnabledWords.Count)];

            if (!ShowNextQuestionHint)
            {
                hintTokenSource.Cancel();
                hintTokenSource.Dispose();
                hintTokenSource = new CancellationTokenSource();
            }
            else
                ShowNextQuestionHint = false;

            SelectedItem = word;
        }

        /// <summary>
        /// Скачивает словари асинхронно
        /// </summary>
        private async void DownloadDictionaries(object allowCancelling)
        {
            if (IsDownloading)
            {
                if (bool.Parse((string)allowCancelling))
                    downloadTokenSource.Cancel();
                return;
            }

            if (PermissionType == DownloadPermissionType.None && !CheckForFileConflicts())
            {
                TaskDialogResult result = TaskDialog.Show(PermissionTaskDialogOptions);
                switch (result.CustomButtonResult)
                {
                    case 0:
                        PermissionType = DownloadPermissionType.Rewrite;
                        break;
                    case 1:
                        PermissionType = DownloadPermissionType.Rename;
                        break;
                    case 2:
                    case null:
                        return;
                }
            }

            IsDownloading = true;
            DownloadedPercent = 0;

            DownloadedBytes.Clear();
            BytesToDownload.Clear();
            UpdateDownloadInfo();
            List<Task<bool>> downloads = new List<Task<bool>>();

            foreach (var keypair in FileHelpers.DefaultDictionariesFiles)
            {
                downloads.Add(DownloadFile(keypair.Key, keypair.Value));
            }

            try
            {
                bool[] results = await Task.WhenAll(downloads.ToArray());
                bool isAnyCancelled = false;
                foreach (bool isFinished in results)
                    if (!isFinished)
                    {
                        isAnyCancelled = true;
                        break;
                    }

                if (isAnyCancelled && DownloadCancellationToken.IsCancellationRequested)
                {
                    downloadTokenSource.Dispose();
                    downloadTokenSource = new CancellationTokenSource();
                    DownloadingToolTipInfo = "Скачавание отменено";
                }
                else
                {
                    DownloadingToolTipInfo = "Скачивание завершено";
                    UpdateDictionaries();
                }
            }
            catch (KeyNotFoundException)
            {
                DownloadingToolTipInfo = "Потеряно соединение с интернетом";
            }

            IsDownloading = false;
        }

        /// <summary>
        /// Скачивает файл асинхронно
        /// </summary>
        /// <param name="url">Адресс, с которого качается файл</param>
        /// <param name="preferedFileName">Предпочитаемое имя файла</param>
        /// <returns>true если задача не отменена</returns>
        private async Task<bool> DownloadFile(string url, string preferedFileName)
        {
            if (File.Exists(Path.Combine(FileHelpers.DictironaryFolderPath, $"{preferedFileName}.txt")) && PermissionType == DownloadPermissionType.Rename)
            {
                int postfix = 1;
                while (File.Exists(Path.Combine(FileHelpers.DictironaryFolderPath, $"{preferedFileName} {postfix}.txt")))
                    postfix++;

                preferedFileName += $" {postfix}";
            }

            long fileSize = await FileHelpers.GetFileSize(url);
            if (fileSize == 0)
                return false;

            DownloadedBytes.Add(preferedFileName, 0);
            BytesToDownload.Add(preferedFileName, fileSize);

            using (WebClient web = new WebClient())
            {
                web.DownloadProgressChanged += (s, e) =>
                {
                    DownloadedBytes[preferedFileName] = e.BytesReceived;

                    UpdateDownloadInfo();
                };
                Task downloadingTask = web.DownloadFileTaskAsync(url, Path.Combine(FileHelpers.DictironaryFolderPath, $"{preferedFileName}.txt"));

                while (!downloadingTask.IsCompleted)
                {
                    if (DownloadCancellationToken.IsCancellationRequested)
                    {
                        web.CancelAsync();
                        return false;
                    }

                    await Task.Delay(500);
                }
            }

            return true;
        }

        /// <summary>
        /// Обновляет в подсказке информацию о скачивании файлов
        /// </summary>
        private void UpdateDownloadInfo()
        {
            long totalBytesToDownload = 0;
            foreach (var entry in BytesToDownload)
                totalBytesToDownload += entry.Value;

            long downloaded = 0;
            foreach (var entry in DownloadedBytes)
                downloaded += entry.Value;

            if (totalBytesToDownload != 0)
                DownloadedPercent = (int)((decimal)downloaded / (decimal)totalBytesToDownload * 100);
            else
                DownloadedPercent = 0;
            DownloadingToolTipInfo = $"Скачано: {FileHelpers.GetSizeReadable(downloaded)} из {FileHelpers.GetSizeReadable(totalBytesToDownload)}\nНажмите для отмены";
        }

        /// <summary>
        /// Проверяет файлы словарей на наличие именных конфликтов со скачиваемыми файлами
        /// </summary>
        /// <returns>true если конфликтов нет</returns>
        private bool CheckForFileConflicts()
        {
            foreach (var keypair in FileHelpers.DefaultDictionariesFiles)
            {
                if (File.Exists(Path.Combine(FileHelpers.DictironaryFolderPath, $"{keypair.Value}.txt")))
                    return false;
            }

            return true;
        }

        #endregion
    }
}
