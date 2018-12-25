using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Command
{
    public class ClearCommand : ICommand
    {
        public string Description
        {
            get
            {
                return "clear the screen";
            }
        }

        public string HelpMessage
        {
            get
            {
                return "<clear>";
            }
        }

        public string Name
        {
            get
            {
                return "clear";
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
                context.StdError.Flush();
                context.StdOut.Flush();
            }

        }
    }
}
