using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI
{
    public class CommandContext
    {
        public TextWriter StdOut { get; set; }

        public TextWriter StdError { get; set; }

    }
}
