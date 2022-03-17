using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Exceptions
{
    public class EntityCountExceededException : Exception
    {
        public EntityCountExceededException(string name, int max) : base($"{name} exceeds the maximum allowed number of {max}.")
        {

        }
    }
}
