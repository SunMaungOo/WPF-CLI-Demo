using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Command
{
    /*

        Command format

        program-name <verb> [options]
        program-name <verb> <target> [options]
        program-name <verb> <source> [options]

        @Source=http://iouri-khramtsov.blogspot.com/2015/04/seeking-perfect-command-line-interface.html
        @Source=http://hackaday.com/2010/08/26/so-you-want-to-make-a-command-line-interface/
        @Source=http://www.codeproject.com/Articles/5839/DIY-Intellisense

    */
    public interface ICommand
    {
        void Run(string[] args, CommandContext context);

        /// <summary>
        /// The name of command which get invoked
        /// </summary>
        string Name { get; }

        string Description { get;}

        string HelpMessage { get;}

    }
}
