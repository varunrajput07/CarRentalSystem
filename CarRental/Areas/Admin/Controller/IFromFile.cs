namespace CarRental.Areas
{
    public interface IFromFile
    {
        ReadOnlySpan<char> Filename { get; }

        void CopyTo(object fileStreams);
    }
}