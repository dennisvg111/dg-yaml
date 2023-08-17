using DG.Yaml.Tokenization;
using Xunit;

namespace DG.Yaml.Tests.Tokenization
{
    public class PlainScalarTokenizerTests : TokenizerTestsBase
    {

        [Fact]
        public void HelloWorld()
        {
            string file = "Hello world!";
            var tokenizer = SetupTokenizerFromString(file);

            tokenizer.TryRead();
            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);

            token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.PlainScalar, token.Type);
            Assert.Equal("Hello world!", token.Content);
        }

        [Fact]
        public void PlainLines()
        {
            string file = "1st non-empty\n\n 2nd non-empty \n\t3rd non-empty";
            var tokenizer = SetupTokenizerFromString(file);

            tokenizer.TryRead();
            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);

            token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.PlainScalar, token.Type);
            Assert.Equal("1st non-empty\n2nd non-empty 3rd non-empty", token.Content);
        }
    }
}
