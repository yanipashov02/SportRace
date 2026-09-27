using System;
using System.Collections.Generic;
using System.Text;

namespace SportRace.Domain.Entities
{
    public class StartListEntry
    {
        public int Id { get; set; }

        public int StartListId { get; set; }
        public StartList StartList { get; set; } = null!;

        public int RegistrationId { get; set; }
        public Registration Registration { get; set; } = null!;

        public int StartNumber { get; set; }

        public DateTime ScheduledStartTime { get; set; }
    }
}