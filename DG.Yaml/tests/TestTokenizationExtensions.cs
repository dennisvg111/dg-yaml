using DG.Yaml.Tokenization;
using FluentAssertions;

namespace DG.Yaml.Tests
{
    public static class TestTokenizationExtensions
    {
        public static void ConsumeStart(this MainTokenizer tokenizer)
        {
            tokenizer.TryRead();

            var startToken = tokenizer.ConsumeToken();

            startToken.Should().NotBeNull();
            startToken.Type.Should().Be(TokenType.StreamStart);
        }
    }
}
