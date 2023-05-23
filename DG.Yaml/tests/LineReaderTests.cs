using System;
using System.IO;
using System.Text;
using Xunit;

namespace DG.Yaml.Tests
{
    public class LineReaderTests
    {
        [Fact]
        public void ReadLine_SingleWorks()
        {
            string expected = "Hello world.";
            var stream = GenerateStreamFromString(expected);
            LineReader reader = new LineReader(stream);

            var actual = reader.ReadLine();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ReadLine_MultipleWorks()
        {
            string expected1 = "Hello world.";
            string expected2 = "this is a test.";
            var stream = GenerateStreamFromString(expected1 + Environment.NewLine + expected2);
            LineReader reader = new LineReader(stream);

            var actual1 = reader.ReadLine();
            var actual2 = reader.ReadLine();

            Assert.Equal(expected1, actual1);
            Assert.Equal(expected2, actual2);
        }

        [Fact]
        public void ReadLine_ClosingNewline()
        {
            string expected1 = "Hello world.";
            var stream = GenerateStreamFromString(expected1 + Environment.NewLine);
            LineReader reader = new LineReader(stream);

            var actual1 = reader.ReadLine();
            var actual2 = reader.ReadLine();

            Assert.Equal(expected1, actual1);
        }

        private static Stream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""), false);
        }
    }
}
