using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ZarasPDFPasswordProtector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileInfo _originalFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetPassword(object sender, RoutedEventArgs e)
        {

            MessageBox.Show(PasswordBox.Text);
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // do something with the filename
                //MessageBox.Show(openFileDialog.FileName);

                _originalFile = new FileInfo(openFileDialog.FileName);

                OriginalFilePath_Text.Text = _originalFile.Name;
            }
        }
    }
}
