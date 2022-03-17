using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Service.Exceptions
{
    public class PinFallsExceededException : Exception
    {
        public PinFallsExceededException(int remainingPins) : base($"Pinfalls can only accomodate up to {remainingPins}.")
        {

        }
    }
}
