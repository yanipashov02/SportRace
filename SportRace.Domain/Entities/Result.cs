namespace SportRace.Domain.Entities
{
    public class Result
    {
        public int Id { get; set; }

        public int RegistrationId { get; set; }
        public Registration Registration { get; set; } = null!;

        public TimeSpan TotalTime { get; set; }

        public int PenaltySeconds { get; set; }

        public TimeSpan FinalTime { get; set; }

        public int Position { get; set; }

        public bool IsFinished { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}