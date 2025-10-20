using System;
using System.Windows.Forms;

namespace Cookbook.Client;

internal static class Program
{
	/// <summary>
	///     The main entry point for the application.
	/// </summary>
	[STAThread]
    private static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        // Gestion des exceptions du thread principal (UI)
        Application.ThreadException += GlobalException.HandleThreadException;

        // Gestion des exceptions non g�r�es (tous threads)
        AppDomain.CurrentDomain.UnhandledException += GlobalException.HandleException;
        Application.Run(new MainForm());
    }
}