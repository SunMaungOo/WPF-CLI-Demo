using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CLI.Command
{
    public class ExitCommand : ICommand
    {
        private Window window;

        public ExitCommand(Window window)
        {
            this.window = window;
        }

        public string Description
        {
            get
            {
                return "exit the CLI program";
            }
        }

        public string HelpMessage
        {
            get
            {
                return "<exit>";
            }
        }

        public string Name
        {
            get
            {
                return "exit";
            }
        }

        public void Run(string[] args, CommandContext context)
        {
            if(args.Length>0)
            {
                context.StdError.WriteLine("Argument should only be 1");
            }
            else
            {
                this.window.Close();
            }

        }
    }
}
