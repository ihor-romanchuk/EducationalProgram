using System;
using System.Collections.Generic;

namespace EducationalProgram_PaskalProcAndFunc
{
    public class TheoryRepository
    {
        public Lazy<List<Tuple<string, string>>> TheoryChapterPathes { get; private set; }

        public TheoryRepository()
        {
            TheoryChapterPathes = new Lazy<List<Tuple<string, string>>>(() => GetTheoryChapterPathes());
        }

        private List<Tuple<string, string>> GetTheoryChapterPathes()
        {
            return new List<Tuple<string, string>>
            {
                new Tuple<string, string>("Вступ", @"../../Resources/Theory/Intro.rtf"),
                new Tuple<string, string>("Загальні відомості", @"../../Resources/Theory/Base info.rtf"),
                new Tuple<string, string>("Оголошення процедур і функцій", @"../../Resources/Theory/FuncAndProcDescription.rtf"),
                new Tuple<string, string>("Виклик процедур і функцій", @"../../Resources/Theory/FuncAndProcCalling.rtf"),
                new Tuple<string, string>("Приклад використання процедури", @"../../Resources/Theory/ProcUsageExampleMaxOfTwoDigits.rtf"),
                new Tuple<string, string>("Приклад використання функції", @"../../Resources/Theory/FuncUsageExampleMaxOfTwoDigits.rtf"),
                new Tuple<string, string>("Види параметрів", @"../../Resources/Theory/ParameterTypes.rtf"),
                new Tuple<string, string>("Область дії імен", @"../../Resources/Theory/LocalAndGlobalIdentifiers.rtf")
            };
        }
    }
}
