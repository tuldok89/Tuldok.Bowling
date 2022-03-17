using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuldok.Bowling.Data.Entities
{
    public class Shot : BaseEntity
    {
        public int FallenPins { get; set; }
        public int ShotNumber { get; set; }
        public Frame Frame { get; set; }
    }
}
