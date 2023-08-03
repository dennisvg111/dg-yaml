namespace DG.Yaml.Parsers
{
    public class FlowScalarParser
    {
        private readonly bool _isSingleQuote;
        private readonly LineReader _reader;

        private readonly Scalar _scalar = new Scalar();

        public FlowScalarParser(LineReader reader, bool isSingleQuote)
        {
            _reader = reader;
            _isSingleQuote = isSingleQuote;

            _scalar = new Scalar();
        }


    }
}
