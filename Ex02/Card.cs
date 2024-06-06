
using Ex02;

namespace GameLogics
{
    public struct Card  // Class?
    {
        private int m_CardId;
        private eCardStatus m_CardStatus;
        // private Card m_pair;

        public Card(int i_CardId)
        {
            m_CardId = i_CardId;
            m_CardStatus = eCardStatus.FacedDown;
        }

        public int ID
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