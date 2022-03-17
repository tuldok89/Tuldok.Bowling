using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string name) : base($"{name} ID not found.")
        {
            
        }
    }
}
