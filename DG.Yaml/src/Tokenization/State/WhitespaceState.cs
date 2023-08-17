using System.Text;

namespace DG.Yaml.Tokenization.State
{
    public class WhitespaceState
    {
        private int _leadingNewlineLength;
        private int _trailingNewlineLength;
        private readonly StringBuilder _whitespaceCharacters = new StringBuilder();
        private bool _isEscapedLeadingNewline;

        public bool IsLeadingBlanks => _isEscapedLeadingNewline || _leadingNewlineLength > 0;
        public int WhitespaceLength => _whitespaceCharacters.Length;

        public bool HasLeadingNewline => _leadingNewlineLength > 0;
        public bool HasTrailingNewline => _trailingNewlineLength > 0;

        public WhitespaceState()
        {

        }

        public void AppendWhitespace(char ch)
        {
            _whitespaceCharacters.Append(ch);
        }

        public void ClearWhitespace()
        {
            _whitespaceCharacters.Clear();
        }

        public string GetWhitespace()
        {
            return _whitespaceCharacters.ToString();
        }

        public void SetLeadingNewlineLength(int leadingNewlineLength)
        {
            _leadingNewlineLength = leadingNewlineLength;
        }

        public void SetTrailingNewlineLength(int trailingNewlineLength)
        {
            _trailingNewlineLength = trailingNewlineLength;
        }

        public void SetIsEscapedLeadingNewline(bool isEscapedLeadingNewline)
        {
            _isEscapedLeadingNewline = isEscapedLeadingNewline;
        }

        public string GetLeadingNewline()
        {
            return ConvertLengthToNewline(_leadingNewlineLength);
        }

        public string GetTrailingNewline()
        {
            return ConvertLengthToNewline(_trailingNewlineLength);
        }

        private static string ConvertLengthToNewline(int length)
        {
            switch (length)
            {
                case 1:
                    return "\n";
                case 2:
                    return "\r\n";
                default:
                    return string.Empty;
            }
        }
    }
}
