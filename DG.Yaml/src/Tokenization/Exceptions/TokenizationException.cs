using DG.Yaml.Tokenization.State;
using System;

namespace DG.Yaml.Tokenization.Exceptions
{
    public class TokenizationException : Exception
    {
        private readonly long _line;
        private readonly int _column;

        public long Line => _line;
        public int Column => _column;

        public TokenizationException(ITokenizationState state, string message) : base($"Exception on line {state.Line}, column {state.CharactersSinceNewline}: " + message)
        {
            _line = state.Line;
            _column = state.CharactersSinceNewline;
        }

        public static TokenizationExceptionThrower WithState(ITokenizationState state)
        {
            return new TokenizationExceptionThrower(state);
        }
    }

    public class TokenizationExceptionThrower
    {
        private readonly ITokenizationState _state;

        public TokenizationExceptionThrower(ITokenizationState state)
        {
            _state = state;
        }

        public void Throw(string message)
        {
            throw new TokenizationException(_state, message);
        }
    }
}
