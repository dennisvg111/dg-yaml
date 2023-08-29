namespace DG.Yaml
{
    public interface ICharacterReader
    {
        bool TryRead(out char character);

        int TryPeek(int count, out char[] chars);
        bool TryPeek(out char character);
    }
}