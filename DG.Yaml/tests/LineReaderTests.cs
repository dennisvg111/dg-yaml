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
            using (var reader = GenerateStreamFromString(expected))
            {

                var actual = reader.ReadLine();

                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void ReadLine_MultipleWorks()
        {
            string expected1 = "Hello world.";
            string expected2 = "this is a test.";
            using (var reader = GenerateStreamFromString(expected1 + Environment.NewLine + expected2))
            {

                var actual1 = reader.ReadLine();
                var actual2 = reader.ReadLine();

                Assert.Equal(expected1, actual1);
                Assert.Equal(expected2, actual2);
            }
        }

        [Fact]
        public void ReadLine_ClosingNewline()
        {
            string expected1 = "Hello world.";
            using (var reader = GenerateStreamFromString(expected1 + Environment.NewLine))
            {

                var actual1 = reader.ReadLine();
                var actual2 = reader.ReadLine();

                Assert.Equal(expected1, actual1);
                Assert.Null(actual2);
            }
        }

        [Fact]
        public void SeekLine_FirstLine()
        {
            string test = "Not this line.";
            string expected = "Hello world.";
            using (var reader = GenerateStreamFromString(
                expected + Environment.NewLine +
                test + Environment.NewLine))
            {
                bool output = reader.SeekLine(0);
                var actual = reader.ReadLine();

                Assert.True(output);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void SeekLine_UnusedLines()
        {
            string test = "Not this line.";
            string expected = "Hello world.";
            using (var reader = GenerateStreamFromString(
                test + Environment.NewLine +
                expected + Environment.NewLine +
                test + Environment.NewLine))
            {
                bool output = reader.SeekLine(1);
                var actual = reader.ReadLine();

                Assert.True(output);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void SeekLine_UsedLines()
        {
            string test = "Not this line.";
            string expected = "Hello world.";
            using (var reader = GenerateStreamFromString(
                test + Environment.NewLine +
                expected + Environment.NewLine +
                test + Environment.NewLine))
            {
                reader.ReadLine();
                reader.ReadLine();
                reader.ReadLine();
                bool output = reader.SeekLine(1);
                var actual = reader.ReadLine();

                Assert.True(output);
                Assert.Equal(expected, actual);
            }
        }

        [Fact]
        public void SeekLine_EndOfStreamReturnsFalse()
        {
            string test = "Not this line.";
            string expected = "Hello world.";
            using (var reader = GenerateStreamFromString(
                test + Environment.NewLine +
                expected + Environment.NewLine +
                test + Environment.NewLine))
            {
                reader.ReadLine();
                bool output = reader.SeekLine(4);

                Assert.False(output);
            }
        }

        [Fact]
        public void SeekLine_NegativeThrows()
        {
            string test = "Not this line.";
            string expected = "Hello world.";
            using (var reader = GenerateStreamFromString(
                test + Environment.NewLine +
                expected + Environment.NewLine +
                test + Environment.NewLine))
            {
                reader.ReadLine();

                Action action = () => reader.SeekLine(-1);

                Assert.Throws<ArgumentException>(action);
            }
        }

        private static LineReader GenerateStreamFromString(string value)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""), false);
            return new LineReader(stream);
        }
    }
}
