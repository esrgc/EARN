using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ESRGC.DLLR.EARN.Helpers
{
  public class Utility
  {
    public static List<string> SecurityQuestions {
      get {
        return new List<string> {
                    "What is your favorite pet's name?",
                    "What is your favorite beverage?",
                    "What is your best friend's name?",
                    "What is your favorite sport team?",
                    "What is the name of the first company you worked for?",
                    "Where did you go on your honeymoon?",
                    "What is your favorite movie?",
                    "What is your father's middle name?",
                    "In what city was your mother born?",
                    "In what city were you born?"
                };
      }
    }

    private static Random random = new Random((int)DateTime.Now.Ticks);
    public static string RandomString(int size) {
      StringBuilder builder = new StringBuilder();
      char ch;
      for (int i = 0; i < size; i++) {
        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
        builder.Append(ch);
      }

      return builder.ToString();
    }   
  }
}