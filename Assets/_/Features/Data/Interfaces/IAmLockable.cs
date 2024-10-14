namespace Data.Runtime
{
    public interface IAmLockable
    {
        public void OnLock();

        public void OnUnlock();
    }
}
