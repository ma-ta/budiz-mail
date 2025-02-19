using System.Windows;
using System.Windows.Input;

namespace BudizMail
{
    /// <summary>
    /// Interaction logic for OAplikaciWindow.xaml
    /// </summary>
    public partial class OAplikaciWindow : Window
    {
        bool easterEgg = false;

        public OAplikaciWindow()
        {
            InitializeComponent();

            titleNazev.Text = "Budiž M@il";
            titleVerze.Text += App.VERZE;
            titleArch.Text += App.DOTNET_INFO + " (" + App.ARCH_INFO + ")";
            imgGithub.ToolTip += " – " + App.GITHUB;
            titleLicence.Text += App.LICENCE;
            titleAutor.Text += String.Format("–{0}  {1}", DateTime.Now.Year, App.AUTOR);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // zavře okno po stisknutí Esc
            if (e.Key.ToString() == "Escape")
            {
                this.Close();
            }
        }

        // akce po otočení kolečkem myši na obrázku loga - Easter Egg
        private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!easterEgg)
            {
                easterEgg = true;
                MessageBox.Show("Web:\n" + App.GITHUB, "O aplikaci", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
