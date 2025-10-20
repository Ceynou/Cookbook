using AntdUI;

namespace Cookbook.Client;

public partial class MainForm : Window
{
    public MainForm()
    {
        Localization.Provider = new Localizer();
        "en-US".SetLanguage();
        InitializeComponent();
    }

    private void label1_Click(object sender, EventArgs e)
    {
    }
}