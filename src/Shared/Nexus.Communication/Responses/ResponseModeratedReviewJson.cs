using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Communication.Responses
{
    public class ResponseModeratedReviewJson
    {
        public int ReviewId { get; set; }
        public string ActionTaken { get; set; }
        public string Mensagem { get; set; }
    }
}
