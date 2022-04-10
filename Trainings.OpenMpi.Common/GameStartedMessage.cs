using Trainings.OpenMpi.Common.Enums;

namespace Trainings.OpenMpi.Common
{
    public class GameStartedMessage
    {
        public int GameId { get; set; }

        public GameType GameType { get; set; }
    }
}
