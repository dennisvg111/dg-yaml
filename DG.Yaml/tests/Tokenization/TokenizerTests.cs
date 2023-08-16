using DG.Yaml.Tokenization;
using System.IO;
using System.Text;
using Xunit;

namespace DG.Yaml.Tests.Tokenization
{
    public class TokenizerTests
    {
        [Fact]
        public void TokensStartWithStreamstart()
        {
            string file = "- one\r\n- two";
            var tokenizer = GenerateTokenizerFromString(file);

            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);
        }

        [Fact]
        public void TokensEndWithStreamEnd()
        {
            string file = "";
            var tokenizer = GenerateTokenizerFromString(file);

            tokenizer.TryRead();
            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);

            token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamEnd, token.Type);
        }

        [Fact]
        public void StringReturnsPlainScalar()
        {
            string file = "Hello world!";
            var tokenizer = GenerateTokenizerFromString(file);

            tokenizer.TryRead();
            tokenizer.TryRead();

            var token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamStart, token.Type);

            token = tokenizer.ConsumeToken();
            Assert.Equal(TokenType.StreamEnd, token.Type);
        }

        private static Tokenizer GenerateTokenizerFromString(string value)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""), false);
            var reader = new CharacterReader(stream);
            return new Tokenizer(reader);
        }
    }
}
