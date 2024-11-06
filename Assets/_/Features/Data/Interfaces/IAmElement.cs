namespace Data.Runtime
{
    public interface IAmElement
    {
        public enum Element
        {
            arcane,
            fire,
            water,
            grass,
        }

        Element spell { get; }
    }
}
