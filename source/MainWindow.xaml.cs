using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BudizMail;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        this.Title = App.NAZEV;
        // po spuštění umístí kurzor do políčka SMTP Server
        tboxSmtpServer.Focus();
    }


    // konstanty, proměnné, třídy
    SolidColorBrush barvaZelena = new SolidColorBrush(Color.FromArgb(0xFF, 0x5E, 0xBF, 0x64));
    SolidColorBrush barvaCervena = new SolidColorBrush(Color.FromArgb(0xFF, 0xE4, 0x4A, 0x4A));
    SolidColorBrush barvaModra = new SolidColorBrush(Color.FromArgb(0xFF, 0x5E, 0x95, 0xBF));

    OdeslatMail odeslatMail = new OdeslatMail();


    // obslužné procedury událostí

    private void BtnOdeslat_Click(object sender, RoutedEventArgs e)
    {
        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        btnOdeslat.IsEnabled = false;
        tboxEventLog.Clear();
        tboxEventLog.Foreground = barvaModra;

        odeslatMail.smtpAdresa = tboxSmtpServer.Text;
        odeslatMail.smtpPort = tboxPort.Text;
        odeslatMail.ssl = chckSsl.IsChecked.GetValueOrDefault();

        odeslatMail.uzivatel = tboxUzivatel.Text;
        odeslatMail.heslo = tboxHeslo.Password;
        odeslatMail.jmenoOdesilatele = tboxJmenoOd.Text;
        odeslatMail.emailOdesilatele = tboxOd.Text;
        odeslatMail.emailPrijemce = tboxKomu.Text;
        odeslatMail.predmet = tboxPredmet.Text;
        odeslatMail.zprava = tboxTextMailu.Text;

        switch (odeslatMail.Posli())
        {
            case true:
                if (odeslatMail.ssl)
                {
                    tboxEventLog.Foreground = barvaZelena;
                    tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss")
                        + "  Úspěšně odesláno! (SSL aktivní)\n\n" + tboxEventLog.Text;
                    tboxEventLog.ScrollToHome();
                }
                else
                {
                    tboxEventLog.Foreground = barvaZelena;
                    tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss")
                        + "  Úspěšně odesláno!\n\n" + tboxEventLog.Text;
                    tboxEventLog.ScrollToHome();
                }
                break;
            case false:
                tboxEventLog.Foreground = barvaCervena;
                tboxEventLog.Text = DateTime.Now.ToString("HH:mm:ss")
                    + "  CHYBA:\n" + odeslatMail.chyba + "\n\n" + tboxEventLog.Text;
                tboxEventLog.ScrollToHome();
                break;
        }

        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        btnOdeslat.IsEnabled = true;

    }

    private void BtnVymazat_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Přejete si skutečně vymazat celý formulář?", "Smazat", MessageBoxButton.OKCancel, MessageBoxImage.Question);
        switch (result)
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
                tboxEventLog.Foreground = barvaModra;
                tboxSmtpServer.Focus();
                break;
        }
    }

    private void ChckSsl_CheckedChanged(object sender, RoutedEventArgs e)
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

    private void TboxEventLog_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        tboxEventLog.SelectAll();
        tboxEventLog.ScrollToHome();
    }

    private void Konec(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Přejete si aplikaci ukončit?", "Konec", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        switch (result)
        {
            case MessageBoxResult.OK:
                Application.Current.Shutdown();
                break;
        }
    }

    private void OAplikaci(object sender, RoutedEventArgs e)
    {
        OAplikaciWindow oAplikaci = new OAplikaciWindow();
        oAplikaci.Owner = Application.Current.MainWindow;
        oAplikaci.ShowDialog();
    }

    private void NacistTxt_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.DefaultExt = ".txt"; // Výchozí přípona souboru
        dlg.Filter = "Textový soubor|*.txt|Všechny soubory|*.*"; // Filtr typu souboru
        try
        {
            if (dlg.ShowDialog() == true)
            {
                tboxTextMailu.Text += "\n<VLOŽENO>\n" + File.ReadAllText(dlg.FileName) + "\n</VLOŽENO>\n";
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
