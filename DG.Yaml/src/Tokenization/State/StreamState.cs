namespace DG.Yaml.Tokenization.State
{
    public class StreamState
    {
        private readonly ICharacterReader _characterReader;

        public StreamState(ICharacterReader characterReader)
        {
            _characterReader = characterReader;
        }



        public bool IsCurrent(string input)
        {
            if (!CanRead || CurrentCharacter != input[0])
            {
                return false;
            }
            return IsNext(input.Substring(1));
        }

        public bool IsNext(string input)
        {
            int count = input.Length;
            if (_characterReader.TryPeek(count, out char[] chars) < count)
            {
                return false;
            }
            for (int i = 0; i < count; i++)
            {
                if (input[i] != chars[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
