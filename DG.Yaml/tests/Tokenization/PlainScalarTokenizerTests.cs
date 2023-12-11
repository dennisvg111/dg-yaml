using DG.Yaml.Tokenization;
using FluentAssertions;
using Xunit;

namespace DG.Yaml.Tests.Tokenization
{
    public class PlainScalarTokenizerTests : TokenizerTestsBase
    {

        [Fact]
        public void ConsumeToken_SingleLine_ShouldBeComplete()
        {
            string file = "Hello world!";
            var tokenizer = SetupTokenizerFromString(file);
            tokenizer.ConsumeStart();

            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.PlainScalar, token.Type);
            Assert.Equal("Hello world!", token.Content);
        }

        [Fact]
        public void PlainLines()
        {
            string file = "1st non-empty\n\n 2nd non-empty \n\t3rd non-empty";
            var tokenizer = SetupTokenizerFromString(file);
            tokenizer.ConsumeStart();

            tokenizer.TryRead();
            var token = tokenizer.ConsumeToken();

            token.Type.Should().Be(TokenType.PlainScalar);
            token.Content.Should().Be("1st non-empty\n2nd non-empty 3rd non-empty");
        }

        [Fact]
        public void ConsumeToken_SingleCrLf_ConvertsCrLfToSpace()
        {
            string file = "1st non-empty\n2nd non-empty";
            var tokenizer = SetupTokenizerFromString(file);
            tokenizer.ConsumeStart();

            tokenizer.TryRead();
            var token = tokenizer.ConsumeToken();

            token.Type.Should().Be(TokenType.PlainScalar);
            token.Content.Should().Be("1st non-empty 2nd non-empty");
        }

        [Fact]
        public void ConsumeToken_DoubleCrLf_ContainsWhiteline()
        {
            string file = "1st non-empty\n\n2nd non-empty";
            var tokenizer = SetupTokenizerFromString(file);
            tokenizer.ConsumeStart();

            tokenizer.TryRead();
            var token = tokenizer.ConsumeToken();

            token.Type.Should().Be(TokenType.PlainScalar);
            token.Content.Should().Be("1st non-empty\n2nd non-empty");
        }

        [Fact]
        public void ConsumeToken_TabPrefix_Removed()
        {
            string file = "1st non-empty\n\t2nd non-empty";
            var tokenizer = SetupTokenizerFromString(file);
            tokenizer.ConsumeStart();

            tokenizer.TryRead();
            var token = tokenizer.ConsumeToken();

            token.Type.Should().Be(TokenType.PlainScalar);
            token.Content.Should().Be("1st non-empty 2nd non-empty");
        }
    }
}
