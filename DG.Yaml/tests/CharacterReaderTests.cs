using System.IO;
using System.Text;
using Xunit;

namespace DG.Yaml.Tests
{
    public class CharacterReaderTests
    {
        private const string _testText = "Hello world!";
        [Fact]
        public void TryRead_MovesForward()
        {
            var reader = SetupReaderFromString();

            Assert.True(reader.TryRead(out char next));
            Assert.Equal('H', next);

            Assert.True(reader.TryRead(out next));
            Assert.Equal('e', next);

            Assert.True(reader.TryRead(out next));
            Assert.Equal('l', next);
        }
        [Fact]
        public void TryRead_ReturnsFalseOnEnd()
        {
            var reader = SetupReaderFromString();
            char next;

            for (int i = 0; i < _testText.Length; i++)
            {
                Assert.True(reader.TryRead(out next));
            }
            Assert.False(reader.TryRead(out next));
        }

        [Fact]
        public void TryPeek_KeepsPosition()
        {
            var reader = SetupReaderFromString();

            Assert.True(reader.TryPeek(out char next));
            Assert.Equal('H', next);

            Assert.True(reader.TryRead(out next));
            Assert.Equal('H', next);

            Assert.True(reader.TryPeek(out next));
            Assert.Equal('e', next);
        }

        [Fact]
        public void TryPeek_Multiple()
        {
            var reader = SetupReaderFromString();

            Assert.True(reader.TryRead(out char next));
            Assert.Equal('H', next);

            Assert.Equal(4, reader.TryPeek(4, out char[] subfix));
            Assert.Equal("ello", new string(subfix));

            Assert.True(reader.TryRead(out next));
            Assert.Equal('e', next);
        }


        private static ICharacterReader SetupReaderFromString()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(_testText), false);
            return new CachedCharacterReader(stream);
        }
    }
}
