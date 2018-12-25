using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Command
{
    public class WriteCommand : ICommand
    {
        public string Description
        {
            get
            {
                return "Command which write something to the program";
            }
        }

        public string HelpMessage
        {
            get
            {
                return "<write> [argument]";
            }
        }

        public string Name
        {
            get
            {
                return "write";
            }
        }

        public void Run(string[] args, CommandContext context)
        {
            if (args.Length == 1)
            {
                context.StdOut.WriteLine(args[0]);
            }
            else
            {
                context.StdError.WriteLine("Argument should only be 1");
            }
        }

    }
}
