using EducationalProgram_PaskalProcAndFunc.Models;
using System.Collections.Generic;

namespace EducationalProgram_PaskalProcAndFunc
{
    public class TestQuestionWithAnswerModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<Pair<string, bool>> Answers { get; set; }
    }
}
