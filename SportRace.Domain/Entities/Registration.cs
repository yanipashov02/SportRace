namespace SportRace.Domain.Entities
{
    public class Registration
    {
        public int Id { get; set; }

        public int ParticipantId { get; set; }
        public Participant? Participant { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int DisciplineId { get; set; }
        public Discipline? Discipline { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int AgeGroupId { get; set; }
        public AgeGroup? AgeGroup { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Pending";
    }
}