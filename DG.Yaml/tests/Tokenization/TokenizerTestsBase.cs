using DG.Yaml.Tokenization;
using System.IO;
using System.Text;

namespace DG.Yaml.Tests.Tokenization
{
    public abstract class TokenizerTestsBase
    {
        protected static MainTokenizer SetupTokenizerFromString(string value)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""), false);
            var reader = new CharacterReader(stream);
            return new MainTokenizer(reader);
        }
    }
}
