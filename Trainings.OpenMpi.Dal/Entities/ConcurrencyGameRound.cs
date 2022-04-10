namespace Trainings.OpenMpi.Dal.Entities
{
    public class ConcurrencyGameRound
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AddCoeff { get; set; }

        public int ConcurrencyGameId { get; set; }

        public int OrderIndex { get; set; }

        public bool IsFinished { get; set; }

        public ConcurrencyGame ConcurrencyGame { get; set; }
    }
}
