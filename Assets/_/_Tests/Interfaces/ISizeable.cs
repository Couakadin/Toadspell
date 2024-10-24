public interface ISizeable
{
    #region Publics

    public enum Size
    {
        none = 0,
        small = 1,
        medium = 2,
        large = 3,
    }

    public Size size { get; }

    #endregion 
}