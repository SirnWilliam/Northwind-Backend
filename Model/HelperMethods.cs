using System.Text;

namespace Practic.Model
{
    public class HelperMethods
    {
        /*
        * Method to check the ID with the order id (11000)
        * 
        */
        public string CheckID(int id)
        {
            string creditCard;
            string expirationDate;
            if (id < 11000 && id % 2 != 0)
            {
                creditCard = "4012000098765439";
                expirationDate = "1221";
            }
            else if (id < 11000 && id % 2 == 0)
            {
                creditCard = "5146312200000035";
                expirationDate = "1222";
            }
            else if (id > 11000 && id % 2 != 0)
            {
                creditCard = "371449635392376";
                expirationDate = "1019";
            }
            else
            {
                creditCard = "3055155515160018";
                expirationDate = "1120";
            }
            return creditCard + "." + expirationDate;
        }

        // Method to encrypt the credit card number and the exp date.
        public string Encrypt(string cc)
        {
            StringBuilder stringBuilder = new StringBuilder(cc);
            for (int i = 0; i < cc.Length - 9; i++)
            {
                stringBuilder[i] = 'D';
            }
            return stringBuilder.ToString();
        }
    }
}
