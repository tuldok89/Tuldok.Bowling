using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tuldok.Bowling.Data.Entities
{
    public class Game : BaseEntity
    {

        public string Name { get; set; }

        public IEnumerable<Frame> Frames { get; set; }
    }
}
