using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image16
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                Process.Run(args[0], args[1]);
            }
        }
    }
}
