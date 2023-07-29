using System.Windows;
using System.Windows.Input;

namespace CodeTypeRanking
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModels.ViewModel(this);
        }

        void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                // TextBox のフォーカスを外す
                Keyboard.ClearFocus();
            }
        }
    }
}
