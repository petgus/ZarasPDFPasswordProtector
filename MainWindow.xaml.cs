using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Win32;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;

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
            SetPasswordOnFile(PasswordBox.Text);
            MessageBox.Show("A password protected copy was created");
        }

        private void SetPasswordOnFile(string password)
        {
            // Open an existing document. Providing an unrequired password is ignored.
            PdfDocument document = PdfReader.Open(_originalFile.FullName);

            PdfSecuritySettings securitySettings = document.SecuritySettings;

            // Setting one of the passwords automatically sets the security level to 
            // PdfDocumentSecurityLevel.Encrypted128Bit.
            securitySettings.UserPassword = password;
            //securitySettings.OwnerPassword = "owner";

            // Don't use 40 bit encryption unless needed for compatibility reasons
            //securitySettings.DocumentSecurityLevel = PdfDocumentSecurityLevel.Encrypted40Bit;

            // Restrict some rights.
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = true;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = true;
            securitySettings.PermitPrint = false;

            // Save the document...
            document.Save(_originalFile.FullName.Replace(".pdf", "_protected.pdf"));
        }

        private void SetCurrentFile(string fullPath)
        {
            _originalFile = new FileInfo(fullPath);
            OriginalFilePath_Text.Text = _originalFile.Name;
        }

        private void SelectFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SetCurrentFile(openFileDialog.FileName);
            }
        }
        
        private void File_Drop(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Assuming we have one file that you care about
                SetCurrentFile(files[0]);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
