using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

public class PySharp
{
    public string SessionId { get; private set; }
    public string PySharpDirectory { get; }

    [DllImport("user32.dll")]
    private static extern int SetWindowText(IntPtr hWnd, string text);

    public PySharp(string sessionId)
    {
        SessionId = sessionId;
        PySharpDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1) + ":\\pysharp";
    }

    public void ClearSession()
    {
        if (Directory.Exists(PySharpDirectory))
        {
            foreach (string file in Directory.GetFiles(PySharpDirectory))
            {
                try
                {
                    if (Path.GetFileNameWithoutExtension(file).ToLower().StartsWith($"pysharp_{SessionId}_message"))
                    {
                        File.Delete(file);
                    }
                }
                catch
                {

                }
            }
        }
        else
        {
            Directory.CreateDirectory(PySharpDirectory);
        }
    }

    public void WriteMessage(string message)
    {
        if (!Directory.Exists(PySharpDirectory))
        {
            Directory.CreateDirectory(PySharpDirectory);
        }

        for (int i = 1; i <= 100; i++)
        {
            try
            {
                if (!File.Exists($"{PySharpDirectory}\\pysharp_{SessionId}_message_{i}_from_csharp_to_py"))
                {
                    File.WriteAllText($"{PySharpDirectory}\\pysharp_{SessionId}_message_{i}_from_csharp_to_py", message);
                    break;
                }
            }
            catch
            {

            }
        }
    }

    public string ReadMessage()
    {
        if (!Directory.Exists(PySharpDirectory))
        {
            Directory.CreateDirectory(PySharpDirectory);
        }

        while (true)
        {
            for (int i = 0; i <= 100; i++)
            {
                try
                {
                    if (File.Exists($"{PySharpDirectory}\\pysharp_{SessionId}_message_{i}_from_csharp_to_py"))
                    {
                        return File.ReadAllText($"{PySharpDirectory}\\pysharp_{SessionId}_message_{i}_from_csharp_to_py");
                    }
                }
                catch
                {

                }
            }
        }
    }

    public void RunScript(string fileName, bool noWindow = false, string parameters = "")
    {
        if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".py"))
        {
            fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".py";
        }

        if (!File.Exists(fileName))
        {
            throw new Exception("The specified script file does not exist.");
        }
        
        if (!Path.GetExtension(fileName).ToLower().Equals(".py"))
        {
            throw new Exception("The specified script file has an invalid extension.");
        }

        Thread thread = new Thread(() =>
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = "cmd.exe";
            startInfo.WindowStyle = noWindow ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal;
            startInfo.CreateNoWindow = noWindow;

            if (parameters == null || parameters.Replace(" ", "").Replace('\t'.ToString(), "") == "")
            {
                startInfo.Arguments = $"/c py \"{fileName}\" {SessionId} {parameters}";
            }
            else
            {
                startInfo.Arguments = $"/c py \"{fileName}\" {SessionId}";
            }

            Process proc = Process.Start(startInfo);
            Thread.Sleep(1000);

            if (!noWindow)
            {
                SetWindowText(proc.MainWindowHandle, $"Python script \"{Path.GetFileName(fileName).ToLower()}\" - Session ID \"{SessionId}\"");
            }
        });

        thread.Priority = ThreadPriority.Highest;
        thread.Start();
    }
}