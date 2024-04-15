using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int? BackgroundId { get; set; }
        public int? WorkspaceId { get; set; }
        public List<List> Lists { get; set; } = new List<List>();
    }
}