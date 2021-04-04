using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ToolBox.Common.Utilities
{
    public class GenerateData
    {

        public static Guid RandomGuid()
        {
            return Guid.NewGuid();
        }

        public static string RandomString()
        {
            return RandomString(3,10);
        }

        public static string RandomString(int minLength, int maxLength)
        {
            //ascii space 32, 
            //ascii ucase 65-90
            //ascii lcase 97-122
            int minAscii = 32;
            int maxAxcii = 122;
            List<char> allowableChars = new List<char>();
            allowableChars.AddRange(" abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray());
            
            //var rnd = new Random();

            int len = rnd.Next(minLength,maxLength+1);

            System.Text.StringBuilder sb = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                char candidate = ' ';
                bool inRange = false;
                while(!inRange)
                {
                    int candidateInt = rnd.Next(minAscii,maxAxcii+1);
                    candidate = (char)candidateInt;
                    inRange = allowableChars.Contains(candidate);
                }
                sb.Append(candidate);
            }

            return sb.ToString();
        }

        public static int RandomInt()
        {
            return RandomInt(1,100);
        }

        public static int RandomInt(int minValue, int maxValue)
        {
            //var rnd = new Random();
            return rnd.Next(minValue, maxValue + 1);
        }

        public static bool RandomBool()
        {
            int pseudoBool = RandomInt(0,1);
            return (pseudoBool == 1);
        }

        public static decimal RandomDecimal()
        {
            int beforeDecPt = RandomInt();
            int afterDecPt = RandomInt();
            return decimal.Parse(beforeDecPt.ToString() + "." + afterDecPt.ToString());

        }

		public static FileInfo CreateRandomFile()
		{
			var fileName = RandomString(15,24) + ".txt";
			var fileContent = RandomString(300,500);
			var fileInfo = new FileInfo(fileName);
			File.WriteAllText(fileInfo.FullName, fileContent);
			return fileIno;
		}

        private static Random _rnd = null;

        private static Random rnd
        {
            get
            {
                if (_rnd == null)
                { _rnd = new Random(); }
                return _rnd;
            }
        }
    }
}