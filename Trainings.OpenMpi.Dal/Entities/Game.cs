using Trainings.OpenMpi.Common.Enums;

namespace Trainings.OpenMpi.Dal.Entities
{
    public class Game
    {
        public int Id { get; set; }

        public bool IsActive { get; set; }

        public GameType Type { get; set; }

        public ConcurrencyGame ConcurrencyGame { get; set; }
        
        public PipelineGame PipelineGame { get; set; }
    }
}
