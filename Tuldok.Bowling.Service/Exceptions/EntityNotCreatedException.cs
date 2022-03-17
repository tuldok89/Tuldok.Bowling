using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Exceptions
{
    public class EntityNotCreatedException : Exception
    {
        public EntityNotCreatedException(string name) : base($"Failed to create a new {name}.")
        {
            
        }
    }
}
