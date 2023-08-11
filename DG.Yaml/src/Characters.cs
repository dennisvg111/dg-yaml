using System.Runtime.CompilerServices;

namespace DG.Yaml
{
    public static class Characters
    {
        public const char Space = ' ';
        public const char Tab = '\t';
        public const char Lf = '\n';
        public const char Cr = '\r';

        public const char SequenceEntry = '-';
        public const char MappingKey = '?';
        public const char MappingValue = ':';
        public const char CollectEntry = ',';

        public const char SequenceStart = '[';
        public const char SequenceEnd = ']';
        public const char MappingStart = '{';
        public const char MappingEnd = '}';

        public const char Comment = '#';
        public const char Anchor = '&';
        public const char Alias = '*';
        public const char Tag = '!';

        public const char Literal = '|';
        public const char Folded = '>';
        public const char SingleQuote = '\'';
        public const char DoubleQuote = '"';
        public const char Directive = '%';

        public const char Escape = '\\';
        public const char Reserved1 = '@';
        public const char Reserved2 = '`';

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFlowStructureIndicator(char c)
        {
            return c == SequenceStart || c == SequenceEnd || c == MappingStart || c == MappingEnd || c == CollectEntry;
        }
    }

    public static class CharacterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty(this char c)
        {
            return c == ' '
                || c == '\t'
                || c == '\r'
                || c == '\n';
        }
    }
}
