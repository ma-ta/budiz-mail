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
using Microsoft.Win32;
using System.IO;

namespace PosliMailWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PosliMail PosliMail = new PosliMail();

        public MainWindow()
        {
            InitializeComponent();

            tboxSmtpServer.Focus();
        }

        private void btnOdeslat_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            btnOdeslat.IsEnabled = false;
            tboxEventLog.Clear();

            PosliMail.smtpAdresa = tboxSmtpServer.Text;
            PosliMail.smtpPort = tboxPort.Text;
            PosliMail.ssl = chckSsl.IsChecked.GetValueOrDefault();

            PosliMail.uzivatel = tboxUzivatel.Text;
            PosliMail.heslo = tboxHeslo.Password;
            PosliMail.jmenoOdesilatele = tboxJmenoOd.Text;
            PosliMail.emailOdesilatele = tboxOd.Text;
            PosliMail.emailPrijemce = tboxKomu.Text;
            PosliMail.predmet = tboxPredmet.Text;
            PosliMail.zprava = tboxTextMailu.Text;

            switch (PosliMail.Posli())
            {
                case true:
                    if (PosliMail.ssl)
                    {
                        tboxEventLog.Foreground = Brushes.Green; //new SolidColorBrush(Color.FromArgb(0xFF, 0xB4, 0xF0, 0xB4));
                        tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss") + "  Úspěšně odesláno! (SSL aktivní)\n\n" + tboxEventLog.Text;
                        tboxEventLog.ScrollToHome();
                    }
                    else
                    {
                        tboxEventLog.Foreground = Brushes.Green; //new SolidColorBrush(Color.FromArgb(0xFF, 0xB4, 0xF0, 0xB4));
                        tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss") + "  Úspěšně odesláno!\n\n" + tboxEventLog.Text;
                        tboxEventLog.ScrollToHome();
                    }
                    break;
                case false:
                    tboxEventLog.Foreground = Brushes.DarkRed; //new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0xb4, 0xb4));
                    tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss") + "  CHYBA:\n" + PosliMail.chyba + "\n\n" + tboxEventLog.Text;
                    tboxEventLog.ScrollToHome();
                    break;
            }

            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            btnOdeslat.IsEnabled = true;

        }

        private void btnVymazat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Přejete si skutečně vymazat celý formulář?", "Potvrzení", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            switch(result)
            {
                case MessageBoxResult.OK:
                    tboxSmtpServer.Text = "";
                    tboxPort.Text = "25";
                    chckSsl.IsChecked = false;
                    tboxUzivatel.Text = "";
                    tboxHeslo.Password = "";
                    tboxJmenoOd.Text = "";
                    tboxOd.Text = "@";
                    tboxKomu.Text = "@";
                    tboxPredmet.Text = "";
                    tboxTextMailu.Text = "";
                    tboxEventLog.Clear();
                    tboxEventLog.SetResourceReference(Control.ForegroundProperty, SystemColors.ControlBrushKey);
                    tboxSmtpServer.Focus();
                break;
            }
        }

        private void chckSsl_CheckedChanged(object sender, RoutedEventArgs e)
        {
            switch (chckSsl.IsChecked)
            {
                case true:
                    tboxPort.Text = "465";
                    break;
                case false:
                    tboxPort.Text = "25";
                    break;

            }
        }

        private void tboxEventLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tboxEventLog.SelectAll();
            tboxEventLog.ScrollToHome();
        }

        private void oAplikaci(object sender, RoutedEventArgs e)
        {
            oAplikaci oAplikaci = new oAplikaci();
            oAplikaci.Show();
        }

        private void NacistTxt_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".txt"; // Výchozí přípona souboru
            dlg.Filter = "Textový soubor|*.txt|Všechny soubory|*.*"; // Filtr typu souboru
            try
            {
                if(dlg.ShowDialog() == true)
                {
                    tboxTextMailu.Text = File.ReadAllText(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                tboxEventLog.Foreground = Brushes.DarkRed; //new SolidColorBrush(Color.FromArgb(0xFF, 0xF0, 0xb4, 0xb4));
                tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss") + "  CHYBA:\n" + ex.Message + "\n\n" + tboxEventLog.Text;
                tboxEventLog.ScrollToHome();
            }
        }
    }
}
