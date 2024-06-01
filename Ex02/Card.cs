
using Ex02;

namespace GameLogics
{
    public struct Card  // Class?
    {
        private char m_Letter; // eNum of letters? 
        private eCardStatus m_CardStatus;
        // private Card m_pair;

        public Card(char i_Letter)
        {
            m_Letter = i_Letter;
            m_CardStatus = eCardStatus.FacedDown;
        }

        public char Letter
        {
            get
            {
                return m_Letter;
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