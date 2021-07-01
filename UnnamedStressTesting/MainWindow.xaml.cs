using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnnamedStressTesting
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            (DataContext as MainWindowViewModel).RightSideWordContainer = rightSideWord;
            (DataContext as MainWindowViewModel).LeftMenuContainer = leftMenuGrid;
            Closed += (DataContext as MainWindowViewModel).SaveDictionariesOnClose;

            (DataContext as MainWindowViewModel).SelectedItem = MainWindowViewModel.EnabledWords[FileHelpers.random.Next(MainWindowViewModel.EnabledWords.Count)]; //Debug
        }

        // костыль (или нет?? :thinking:)
        private void WordsTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (DataContext as MainWindowViewModel).SelectedItem = (WordViewModel)(sender as TreeView).SelectedItem;
        }
    }
}
