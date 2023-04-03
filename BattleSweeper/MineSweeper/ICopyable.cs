namespace Games
{
    /// <summary>
    /// interface which copyable objects should implement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICopyable<T> where T : class?
    {
        /// <summary>
        /// should return a copy of the current object,
        /// which does NOT share address with the same object.
        /// </summary>
        T Copy();
    }
}