using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace EducationalProgram_PaskalProcAndFunc
{
    public class DocumentLoader
    {
        public FlowDocument LoadDocument(string documentPath)
        {
            var result = new FlowDocument();
            if (File.Exists(documentPath))
            {
                using (var stream = File.OpenRead(documentPath))
                {
                    var range = new TextRange(result.ContentStart, result.ContentEnd);
                    range.Load(stream, DataFormats.Rtf);
                }
            }

            return result;
        }
    }
}
