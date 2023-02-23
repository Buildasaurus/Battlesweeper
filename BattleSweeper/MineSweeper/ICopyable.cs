namespace Games
{
    public interface ICopyable<T> where T : class?
    {
        T Copy();
    }
}