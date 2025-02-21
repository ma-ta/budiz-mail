using System.Windows;

using System.Runtime.InteropServices;  // pro RuntimeInformation (info o architektuře)


namespace BudizMail;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static string NAZEV = "Budiž Mail";
    public static string AUTOR = "Martin TÁBOR";
    public static string LICENCE = "GNU GPLv3+";
    public static string VERZE = "1.0.0  |  2025-02-22";
    public static string GITHUB = "github.com/ma-ta/budiz-mail";
    public static string DOTNET_INFO = RuntimeInformation.FrameworkDescription;
    public static string ARCH_INFO = RuntimeInformation.ProcessArchitecture.ToString().ToLower();
}
