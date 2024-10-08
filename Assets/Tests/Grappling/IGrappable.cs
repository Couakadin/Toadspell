public interface IGrappable
{
    #region Publics

    /// <summary>
    /// Each sizes of a grappable object
    /// Small: the object will be moved to the player
    /// Large: the player will be moved to the object
    /// </summary>
    public enum Size
    {
        Small = 0,
        Large = 1
    }

    /// <summary>
    /// Parameter to get the size of the object
    /// </summary>
    Size m_grapSize { get; }

    #endregion
}