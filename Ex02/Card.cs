
namespace GameLogics
{
    public struct Card
    {
        private readonly int r_CardId;
        private eCardStatus m_CardStatus;

        public Card(int i_CardId)
        {
            r_CardId = i_CardId;
            m_CardStatus = eCardStatus.FacedDown;
        }

        public int Id
        {
            get
            {
                return r_CardId;
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