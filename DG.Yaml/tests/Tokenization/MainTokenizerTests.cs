using DG.Yaml.Tokenization;
using FluentAssertions;
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
            token.Type.Should().Be(TokenType.StreamStart);
        }

        [Fact]
        public void TokensEndWithStreamEnd()
        {
            string file = "";
            var tokenizer = SetupTokenizerFromString(file);
            tokenizer.ConsumeStart();

            //read next.
            tokenizer.TryRead();

            Token lastToken = null;
            do
            {
                lastToken = tokenizer.ConsumeToken();
            } while (tokenizer.TryRead());

            lastToken.Should().NotBeNull();
            lastToken.Type.Should().Be(TokenType.StreamEnd);
        }
    }
}
