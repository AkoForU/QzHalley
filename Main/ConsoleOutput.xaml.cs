using Main.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Main
{
    /// <summary>
    /// Interaction logic for ConsoleOutput.xaml
    /// </summary>
    public partial class ConsoleOutput : Window
    {
        private readonly QuizDbContext _dbContext;
        private Process _serverProcess;
        private bool _isServerRunning;
        public ConsoleOutput()
        {
            InitializeComponent();
            ResizeMode=ResizeMode.NoResize;
        }
        public void AppendOutput(string text)
        {
            OutputText.Text += text + Environment.NewLine;
        }
        public void ServerStart()
        {
            if (!_isServerRunning)
            {
                _serverProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = "run --project ../../../../QzHalleyAPI/QzHalleyAPI.csproj",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                _serverProcess.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            AppendOutput(e.Data);
                        });
                    }
                };

                _serverProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            AppendOutput("Error: " + e.Data);
                        });
                    }
                };

                _serverProcess.Start();
                _serverProcess.BeginOutputReadLine();
                _serverProcess.BeginErrorReadLine();
                _isServerRunning = true;
                AppendOutput("Server started.");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _serverProcess.Kill();
            _serverProcess.Dispose();
            _isServerRunning = false;
            AppendOutput("Server stopped.");
        }
    }
}
