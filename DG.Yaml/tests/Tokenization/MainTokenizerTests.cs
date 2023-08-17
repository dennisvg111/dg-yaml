using DG.Yaml.Tokenization;
using Xunit;

namespace DG.Yaml.Tests.Tokenization
{
    public class MainTokenizerTests : TokenizerTestsBase
    {
        [Fact]
        public void TokensStartWithStreamstart()
        {
            string file = "";
            var tokenizer = SetupTokenizerFromString(file);

            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);
        }

        [Fact]
        public void TokensEndWithStreamEnd()
        {
            string file = "";
            var tokenizer = SetupTokenizerFromString(file);

            tokenizer.TryRead();
            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);

            token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamEnd, token.Type);
        }
    }
}
