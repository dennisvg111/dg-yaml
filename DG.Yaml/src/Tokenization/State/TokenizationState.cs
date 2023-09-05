namespace DG.Yaml.Tokenization.State
{
    public class TokenizationState
    {
        private readonly StreamState _streamState;

        public TokenizationState(CharacterReader reader)
        {
            _streamState = new StreamState(reader);
        }
    }
}
