using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainings.OpenMpi.Common.Enums;

namespace Trainings.OpenMpi.Dal.Entities
{
    public class PipelineStep
    {
        public int Id { get; set; }
        
        public int PipelineGameId { get; set; }
        
        public int UserId { get; set; }

        public int QuizQuestionId { get; set; }

        public PipelineState State { get; set; }
        
        public SimpleOperationType Operation { get; set; }
        
        public PipelineGame PipelineGame { get; set; }
        
        public User User { get; set; }

        public QuizQuestion QuizQuestion { get; set; }
        public decimal DataValue { get; set; }
    }
}
