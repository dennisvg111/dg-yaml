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
            var reader = GenerateCharacterReaderFromString(file);
            var tokenizer = new Tokenizer(reader);

            tokenizer.TryRead();

            var token = tokenizer.Tokens.Dequeue();
            Assert.Equal(TokenType.StreamStart, token.Type);
        }

        private static CharacterReader GenerateCharacterReaderFromString(string value)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""), false);
            return new CharacterReader(stream);
        }
    }
}
