using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tuldok.Bowling.Data.Entities
{
    public class Frame : BaseEntity
    {
        public Game Game { get; set; }
        [ForeignKey("Game")]
        public Guid GameId { get; set; }
        public int SequenceNumber { get; set; }
        public IEnumerable<Shot> Shots { get; set; }
    }
}
