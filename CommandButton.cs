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
    internal class CommandExecutor
    {
        private readonly Controls.Button buttonRef;
        private string commandLiteral;
        private string argumentLiteral;

        public CommandExecutor(CommandView viewRef, string commandLiteral, string argumentLiteral)
        {
            this.buttonRef = viewRef.GetButton();
            this.commandLiteral = commandLiteral;
            this.argumentLiteral = argumentLiteral;

            init();
        }

        private void init()
        {
            buttonRef.Click += onClick;
            buttonRef.Content = getCommandToExecute();
        }
        
        public void UpdateCommand(string commandLiteral, string argumentLiteral)
        {
            this.commandLiteral = commandLiteral;
            this.argumentLiteral = argumentLiteral;

            buttonRef.Content = getCommandToExecute();
        }

        private string getCommandToExecute()
        {
            return commandLiteral + " " + argumentLiteral;
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(commandLiteral, argumentLiteral);
            Process process = Process.Start(processStartInfo);

            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler((senderer, e) => {
                MessageBox.Show(getCommandToExecute() + "\nを実行しました", "コマンド実行");
            });
        }

    }
}
