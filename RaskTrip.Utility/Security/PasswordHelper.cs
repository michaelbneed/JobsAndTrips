
#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace RaskTrip.Utility.Security
{
    public class PasswordHelper
    {
        public static string GeneratePasswordFromWordList(List<string> wordList)
        {
            
            string word1 = GetRandomWord(wordList);
            word1 = word1.Substring(0, 1).ToUpper() + word1.Substring(1);
            string word2 = GetRandomWord(wordList);
            word2 = word2.Substring(0, 1).ToUpper() + word2.Substring(1);
            int number = GetRandomNumber();

            return BuildPassword(word1, word2, number.ToString("00"));

        }

        private static Random _random = new Random();

        private static string BuildPassword(string word1, string word2, string number)
        {
            int passwordStyle = _random.Next(0, 5);
            switch (passwordStyle)
            {
                case 0:
                    return word1 + word2 + number;
                case 1:
                    return number + word1 + word2;
                case 2:
                    return word1 + number + word2;
                case 3:
                    return word2 + word1 + number;
                case 4:
                    return number + word2 + word1;
                case 5:
                    return word2 + number + word1;
                default:
                    return word1 + word2;
            }
        }

        private static int GetRandomNumber()
        {
            return _random.Next(0, 99);
        }

        private static string GetRandomWord(List<string> wordList)
        {
            return wordList[_random.Next(0, wordList.Count - 1)];
        }

        public static byte[] EncryptPassword(byte[] unencryptedPassword, bool reversible)
        {
            Cryptographer cryptographer = new Cryptographer();
            return cryptographer.EncryptString(unencryptedPassword, reversible);
        }

        public static string DecryptPassword(string base64EncodedPassword)
        {
            byte[] encrypted = Convert.FromBase64String(base64EncodedPassword);
            byte[] plainBytes = DecryptPassword(encrypted);
            return Encoding.Unicode.GetString(plainBytes);
        }
        public static byte[] DecryptPassword(byte[] encodedPassword)
        {
            Cryptographer cryptographer = new Cryptographer();
            return cryptographer.DecryptString(encodedPassword);
        }
    }
}
