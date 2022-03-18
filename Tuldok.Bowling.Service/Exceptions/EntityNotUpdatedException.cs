using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Exceptions
{
    public class EntityNotUpdatedException : Exception
    {
        public EntityNotUpdatedException(string name) : base($"Failed to update {name}.")
        {

        }
    }
}
