namespace Trainings.OpenMpi.Dal.Entities
{
    public class ConcurrencyGame
    {
        public int GameId { get; set; }

        public long CorrectSum { get; set; }

        public long CurrentSum { get; set; }

        public Game Game { get; set; }

        public ICollection<ConcurrencyGameRound> ConcurrencyGameRounds { get; set; }
    }
}
