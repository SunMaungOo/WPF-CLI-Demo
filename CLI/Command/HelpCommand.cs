using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Command
{
    public class HelpCommand : ICommand
    {
        private ICommand[] commandList;

        public HelpCommand()
        {
        }

        public void Set(ICommand[] commandList)
        {
            this.commandList = commandList;
        }

        public string Description
        {
            get
            {
                return "show command list";
            }
        }

        public string HelpMessage
        {
            get
            {
                
                return "<help>\n <help> [command name]";
            }
        }

        public string Name
        {
            get
            {
                return "help";
            }
        }

        public void Run(string[] args, CommandContext context)
        {

            if(args.Length==0)
            {
                context.StdOut.WriteLine("command name:description:usage");

                foreach (ICommand command in commandList)
                {
                    context.StdOut.WriteLine(command.Name + ":" + command.Description + ":" + command.HelpMessage);
                }

            }
            else if(args.Length==1)
            {
                ICommand command = SearchCommand(args[0]);

                if(command!=null)
                {
                    context.StdOut.WriteLine(command.Name + ":" + command.Description + ":" + command.HelpMessage);
                }
                else
                {
                    context.StdError.WriteLine("<help>: cannot find command name("+args[0]+")");
                }

            }
            else if(args.Length>1)
            {
                context.StdError.WriteLine("<help> command only accept one argument");
            }

            
        }

        /// <summary>
        /// Return null if the command is not found
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        private ICommand SearchCommand(string commandName)
        {
            foreach(ICommand command in commandList)
            {
                if(command.Name==commandName)
                {
                    return command;
                }
            }

            return null;
        }

       

      
    }
}
