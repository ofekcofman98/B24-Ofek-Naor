namespace GameLogics
{
    public struct CardsWithIndex
    {
        private int m_CardId;
        private int m_Row;
        private int m_Column;


        public CardsWithIndex(int i_CardId, int i_Row, int i_Column)
        {
            m_CardId = i_CardId;
            m_Row = i_Row;
            m_Column = i_Column;
        }


        public int Id
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
        public int Row
        {
            get
            {
                return m_Row;
            }
            set
            {
                m_Row = value;
            }
        }
        public int Column
        {
            get
            {
                return m_Column;
            }
            set
            {
                m_Column = value; 
            }
        }

    }
}
