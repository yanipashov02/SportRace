using System;
using System.Collections.Generic;
using System.Text;

namespace SportRace.Domain.Entities
{
    public class StartList
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsPublished { get; set; }

        public ICollection<StartListEntry> Entries { get; set; } = new List<StartListEntry>();
    }
}
