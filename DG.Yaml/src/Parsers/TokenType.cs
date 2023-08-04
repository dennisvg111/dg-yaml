namespace DG.Yaml.Parsers
{
    public enum TokenType
    {
        Unknown,
        StreamStart,
        StreamEnd,
        VersionDirective,
        TagDirective,
        DocumentStart,
        DocumentEnd,
        BlockSequenceStart,
        BlockMappingStart,
        BlockEnd,
        FlowSequenceStart,
        FlowSequenceEnd,
        FlowMappingStart,
        FlowMappingEnd,
        BlockEntryStart,
        FlowEntryStart,
        KeyStart,
        ValueStart,
        Alias,
        Anchor,
        Tag,
        PlainScalar,
        SingleQuotedScaler,
        DoubleQuotedScaler,
        LiteralScalar,
        FoldedScalar,
    }

    public static class TokenTypeExtensions
    {
        public static bool IsScalar(this TokenType type)
        {
            return type == TokenType.PlainScalar
                || type == TokenType.SingleQuotedScaler
                || type == TokenType.DoubleQuotedScaler
                || type == TokenType.LiteralScalar
                || type == TokenType.FoldedScalar;
        }
    }
}
