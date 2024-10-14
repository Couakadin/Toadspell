namespace Data.Runtime
{
    public interface IStateMachine
    {
        #region Methods

        /// <summary>
        ///  When enter this state.
        /// </summary>
        public void Enter();

        /// <summary>
        /// // When exist this state.
        /// </summary>
        public void Exit();

        /// <summary>
        /// When in this state each frame.
        /// </summary>
        public void Tick();

        /// <summary>
        /// When in this state without frame.
        /// </summary>
        public void FixedTick();

        /// <summary>
        /// When in this state each end of frame.
        /// </summary>
        public void LateTick();

        #endregion
    }
}
