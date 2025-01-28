using System.Collections.Generic;

namespace demoProject.Models
{
    public class TriviaResponse
    {
        public int ResponseCode { get; set; }
        public List<TriviaQuestion> Results { get; set; } = new List<TriviaQuestion>();
    }
}
