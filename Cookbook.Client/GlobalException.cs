using Cookbook.SharedData;

namespace Cookbook.Client;

public static class GlobalException
{
    public static void HandleThreadException(object sender, ThreadExceptionEventArgs e)
    {
        var ex = e.Exception;
        MessageBox.Show(GetErrorMessage(ex), "Application error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void HandleException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = e.ExceptionObject as Exception;
        MessageBox.Show(GetErrorMessage(ex), "Application error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static string GetErrorMessage(Exception? ex)
    {
        if (ex is null)
            return "Unknown error occured.";

        string message;

        if (ex is RestClientException restEx)
        {
            if (restEx.HasRawContent)
            {
                if (restEx.GetRawContent(out ErrorResponse? error))
                    message = $"{restEx.Message}\n\n{error?.Error}\n\n{error?.Details}";
                else
                    message = $"{restEx.Message}\n\nRaw response content :\n\n{restEx.GetRawContent()}";
            }
            else
            {
                message = restEx.Message;
            }
        }
        else
        {
            message = $"An error occured. :\n\n{ex.Message}";
        }

        return message;
    }
}