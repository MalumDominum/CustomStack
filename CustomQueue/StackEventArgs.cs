using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomStack
{
    public class StackEventArgs : EventArgs
    {
        public string Message { get; set; }

        public StackEventArgs(string message)
        {
            Message = message;
        }
    }
}
