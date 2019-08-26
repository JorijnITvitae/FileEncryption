using System;
using System.IO;
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
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt(dialog.FileName);
                MessageBox.Show(File.ReadAllText(dialog.FileName));
            }
        }

        private void Button_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                EncryptDecrypt encryptDecrypt = new EncryptDecrypt(dialog.FileName);
                MessageBox.Show(File.ReadAllText(dialog.FileName));
            }
        }

        private void TextBox_Password_Initialized(object sender, EventArgs e)
        {
            TextBox_Password.Text = "Password";
            TextBox_Password.Foreground = Brushes.Silver;
        }

        private void TextBox_Password_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox_Password.Foreground == Brushes.Silver)
            {
                TextBox_Password.Text = "";
                TextBox_Password.Foreground = Brushes.Black;
            }
        }

        private void TextBox_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBox_Password.Text == "")
            {
                TextBox_Password.Text = "Password";
                TextBox_Password.Foreground = Brushes.Silver;
            }
        }
    }
}
