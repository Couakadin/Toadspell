namespace Data.Runtime
{
    public interface ILockable
    {
        public void OnLock();

        public void OnUnlock();
    }
}
