
namespace GameLogics
{
    public class Player
    {
        private string m_PlayerName;
        private int m_Score;
        private bool m_IsComputer;

        public string Name
        {
            get
            {
                return m_PlayerName;
            }
            set
            {
                m_PlayerName = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }
            set
            {
                m_Score = value;
            }
        }

        public bool IsComputer
        {
            get
            {
                return m_IsComputer;
            }
            set
            {
                m_IsComputer = value;
            }
        }
    }
}
