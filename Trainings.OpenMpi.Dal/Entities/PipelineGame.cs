namespace Trainings.OpenMpi.Dal.Entities
{
    public class PipelineGame
    {
        public int GameId { get; set; }
        public int PipelineLength { get; set; }

        public Game Game { get; set; }

        public ICollection<PipelineStep> PipelineSteps { get; set; }
        public int RoundsLeft { get; set; }
    }
}
