namespace Enemies.Runtime
{
    public class StateMachine : AState
    {
        public BossBehaviour m_bossBehaviour { get; }

        public BossState m_bossState;
        public CollapseState m_collapseState;
        public LineState m_lineState;
        public ZoneState m_zoneState;

        public StateMachine(BossBehaviour bossBehaviour)
        {
            m_bossBehaviour = bossBehaviour;

            m_bossState = new(this);
            m_collapseState = new(this);
            m_lineState = new(this);
            m_zoneState = new(this);
        }
    }
}