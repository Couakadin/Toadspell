namespace Data.Runtime
{
    public interface IAmSpellGiver
    {
        public enum Spell
        {
            arcane,
            fire,
            water,
            grass,
        }

        Spell spell { get; }
    }
}
