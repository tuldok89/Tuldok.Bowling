using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Exceptions
{
    public class DuplicateSequenceException : Exception
    {
        public DuplicateSequenceException(string name) : base($"{name} has a duplicate sequence number.")
        {

        }
    }
}
