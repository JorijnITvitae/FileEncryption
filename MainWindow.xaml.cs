using System.Windows;
using Microsoft.Win32;

namespace FileEncryption
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Encrypt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                new EncryptDecrypt(dialog.FileName, EncryptDecrypt.mode.ENCRYPT);
            }
        }

        private void Button_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                new EncryptDecrypt(dialog.FileName, EncryptDecrypt.mode.DECRYPT);
            }
        }
    }
}
