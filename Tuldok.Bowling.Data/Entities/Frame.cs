using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tuldok.Bowling.Data.Entities
{
    public class Frame : BaseEntity
    {
        public Game Game { get; set; }
        public int FrameNumber { get; set; }
        public IEnumerable<Shot> Shots { get; set; }
    }
}
