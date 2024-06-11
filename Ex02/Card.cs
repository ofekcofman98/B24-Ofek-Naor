
namespace GameLogics
{
    public struct Card
    {
        private int m_CardId;
        private eCardStatus m_CardStatus;

        public Card(int i_CardId)
        {
            m_CardId = i_CardId;
            m_CardStatus = eCardStatus.FacedDown;
        }

        public int Id
        {
            get
            {
                return m_CardId;
            }
        }

        public eCardStatus CardStatus
        {
            get
            {
                return m_CardStatus;
            }
            private set
            {
                m_CardStatus = value;
            }
        }

        public bool IsFacedUp()
        {
            return (eCardStatus.CurrentlyFacedUp == CardStatus) || (eCardStatus.PermanentlyFacedUp == CardStatus);
        }

        public void FlipUp()
        {
            CardStatus = eCardStatus.CurrentlyFacedUp;
        }

        public void FlipDown()
        {
            CardStatus = eCardStatus.FacedDown;
        }

        public void RevealPermanently()
        {
            CardStatus = eCardStatus.PermanentlyFacedUp;
        }
    }
}