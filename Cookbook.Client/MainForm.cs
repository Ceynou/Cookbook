using AntdUI;
using Button = AntdUI.Button;
using Label = AntdUI.Label;

namespace Cookbook.Client;

public partial class MainForm : Window
{
    public MainForm()
    {
        Localization.Provider = new Localizer();
        "en-US".SetLanguage();
        InitializeComponent();


        PageHeader pageHeader = new()
        {
            Dock = DockStyle.Top,
            ShowButton = true,
            ShowBack = true,
            ShowIcon = true
        };
        Controls.Add(pageHeader);

        VirtualPanel virtualPanel = new()
        {
            Dock = DockStyle.Fill,
            AlignItems = TAlignItems.Center,
            JustifyContent = TJustifyContent.Center
        };
        Controls.Add(virtualPanel);

        VirtualPanel virtualPanel2 = new();
        virtualPanel2.Controls.Add(new VirtualPanel());
        virtualPanel.Controls.Add(virtualPanel2);

        Label usernameLabel = new();
        usernameLabel.Text = "username";
        virtualPanel2.Controls.Add(usernameLabel);

        Input usernameInput = new();
        virtualPanel2.Controls.Add(usernameInput);

        Label passwordLabel = new();
        passwordLabel.Text = "password";
        virtualPanel2.Controls.Add(passwordLabel);

        Input passwordInput = new();
        virtualPanel2.Controls.Add(passwordInput);

        Button logIn = new();
        logIn.Text = "Log In";
        virtualPanel2.Controls.Add(logIn);
    }
}