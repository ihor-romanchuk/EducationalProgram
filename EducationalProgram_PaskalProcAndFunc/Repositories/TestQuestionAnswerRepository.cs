using System;
using System.Collections.Generic;

namespace EducationalProgram_PaskalProcAndFunc
{
    public class TestQuestionAnswerRepository
    {
        private const string TestsPath = @"../../Resources/Tests.xml";

        private readonly XmlHelper _xmlHelper;

        public Lazy<List<TestQuestionWithAnswerModel>> Questions { get; private set; }

        public TestQuestionAnswerRepository()
        {
            _xmlHelper = new XmlHelper();
            Questions = new Lazy<List<TestQuestionWithAnswerModel>>(() => GetQuestions());
        }

        private List<TestQuestionWithAnswerModel> GetQuestions()
        {
            return _xmlHelper.ReadFromXML<List<TestQuestionWithAnswerModel>>(TestsPath);
        }
    }
}
