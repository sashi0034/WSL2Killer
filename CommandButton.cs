using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Controls = System.Windows.Controls;

namespace WSL2Killer
{
    record class CommandLine(string Name, string Argment)
    {
        public string GetLine()
        {
            return Name + " " + Argment;
        }
    }


    internal class CommandExecutor
    {
        private readonly Controls.Button buttonRef;
        private CommandLine[] commandQue;

        public CommandExecutor(CommandView viewRef, CommandLine[] commandQue)
        {
            this.buttonRef = viewRef.GetButton();
            this.commandQue = commandQue;
            init();
        }

        private void init()
        {
            buttonRef.Click += onClick;
            buttonRef.Content = getCommandToExecute();
        }
        
        public void UpdateCommand(CommandLine[] commandQue)
        {
            this.commandQue = commandQue;

            buttonRef.Content = getCommandToExecute();
        }

        private string getCommandToExecute()
        {
            string result = "";
            for (int i=0; i<commandQue.Length; ++i)
            {
                var nextLine = commandQue[i];
                result += nextLine.GetLine();
                if (i == commandQue.Length - 1) break;
                result += "\n";
            }

            return result;
        }

        private async void onClick(object sender, RoutedEventArgs e)
        {
            var numLine = commandQue.Length;
            for (int i=0; i<numLine; ++i)
            {
                await executeLine(numLine, i);
            }
        }

        private async Task executeLine(int numLine, int index)
        {
            var nextLine = commandQue[index];
            ProcessStartInfo processStartInfo = new ProcessStartInfo(nextLine.Name, nextLine.Argment);
            Process process = Process.Start(processStartInfo);

            process.EnableRaisingEvents = true;

            var onFinish = new TaskCompletionSource<Unit>();

            process.Exited += new EventHandler((senderer, e) =>
            {
                if (index == numLine - 1) MessageBox.Show(getCommandToExecute() + "\nを実行しました", "コマンド実行");
                onFinish.SetResult(Unit.Default);
            });

            await onFinish.Task;
        }
    }
}
