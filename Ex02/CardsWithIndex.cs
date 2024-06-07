using Ex02;

namespace GameLogics
{
    public struct CardsWithIndex
    {
        private int? m_CardId;
        private int? m_Index1;
        private int? m_Index2;


        public CardsWithIndex(int i_CardId, int i_Index1, int i_Index2)
        {
            m_CardId = i_CardId;
            m_Index1 = i_Index1;
            m_Index2 = i_Index2;
        }


        public int? ID
        {
            get
            {
                return m_CardId;
            }
            set
            {
                m_CardId = value; 
            }
        }
        public int? Index1
        {
            get
            {
                return m_Index1;
            }
            set
            {
                m_Index1 = value;
            }
        }
        public int? Index2
        {
            get
            {
                return m_Index2;
            }
            set
            { 
                m_Index2 = value; 
            }
        }
    }
}
