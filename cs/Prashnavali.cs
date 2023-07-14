using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RamShalaka
{
    public class Prashnavali
    {
        char[] charSet = new char[] { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','`','~','!','@','#','$','%','^','&','(',')','_','{','}','[',']','|','\\',':',';','"','.','?',',','+','-','*','/','<','>','=','\'','0','1','2','3','4','5','6','7','8','9' };

        List<int> Key;
        List<int> Key2;

        public string Encrypt(string textToEncrypt,int key2)
        {
            var charArray = textToEncrypt.ToCharArray();
            var words = textToEncrypt.Split();
            var wordsCount = words.Count();
            var count = charArray.Count();
            var avg = count / words.Count();
            var root = Math.Round(Math.Sqrt(count));
            var nearest_sq = Math.Pow(root, 2);
            if(nearest_sq < count)
            {
                root++;
                nearest_sq = Math.Pow(root, 2);
            }

            var random = new Random();
            var select = random.Next(avg, 10);
            var square = Convert.ToInt32(nearest_sq);
            List<char> TransposedText = new List<char>();
            var range = new char[square];
            TransposedText.InsertRange(0, range);
            for (int j=0;j<words.Count();j++)
            {
                var wordCharArray = words[j].ToCharArray();
                var wordCount = j;
                
                for (int i = 0; i < wordCharArray.Count();i++)
                {
                    if(wordCount >= TransposedText.Count())
                    {
                        TransposedText.AddRange(new char[select]);
                    }
                    TransposedText[wordCount] = wordCharArray[i];
                    wordCount = wordCount + select;
                }
            }

            var TransposedTextCount = TransposedText.Count();

            Key = new List<int>(new int[TransposedTextCount]);
            List<char> Result = new List<char>(new char[TransposedTextCount]);
            var rand = new Random();
            for (int i=0;i< TransposedTextCount;i++)
            {
                var text = char.ToLower(TransposedText[i]);
                var data = Array.IndexOf(charSet, text);
                
                Key[i] = rand.Next(100, 500);
                var mod = (data + Key[i]) % charSet.Count();
                Result[i] = charSet[mod];

            }

            var wordsRoot = Math.Round(Math.Sqrt(wordsCount));
            var wordsSquare = ((uint)Math.Pow(wordsRoot, 2));
            if(wordsSquare < wordsCount)
            {
                wordsRoot++;
                wordsSquare = ((uint)Math.Pow(wordsRoot, 2));
            }
            Key2 = new List<int>(new int[wordsSquare]);
            for(int i=0;i<wordsCount;i++)
            {
                Key2[i] = key2 + words[i].Count();
            }

            return new string( Result.ToArray());
        }

        private int ReverseModulus(int div, int a, int remainder)
        {
            if (remainder >= div)
                throw new ArgumentException("Remainder cannot be greater than or equal to divisor");
            if (a < remainder)
                return remainder - a;
            return div + remainder - a;
        }

        public string Decrypt(string textToDecrypt,int key2)
        {
            List<char> textArray = new List<char>();
            
            var charArray = textToDecrypt.ToCharArray();
            int count = 0;
            List<int> keys = new List<int>();
            foreach (var key in Key2)
            {
                if (key != 0)
                {
                    var newKey = (key - key2);
                    count = count + newKey;
                    keys.Add(newKey);
                }
            }

            var root = Math.Round(Math.Sqrt(count));
            var nearest_sq = Math.Pow(root, 2);
            if (nearest_sq < count)
            {
                root++;
                nearest_sq = Math.Pow(root, 2);
            }

            for (int i =0;i<charArray.Count();i++)
            {
                var data = Array.IndexOf(charSet, charArray[i]);
                data = data + 68;
                var result = data - Key[i];
                while(result < 0)
                {
                    result = result + 68;
                }
                textArray.Add(charSet[result]);
            }

            List<string> words = new List<string>();

            //var sub = textArray.Count() % root;
            var height = Math.Round(textArray.Count() / root);
            

            var hw = height + root;
            var colrow = textArray.Count() + count;
            var col = (colrow - hw);
            var nvalue  = (int)(col / hw);
            nvalue = keys.Count() * nvalue;
            //var select = textArray.Count() % avg;

            for(int i = 0; i < keys.Count(); i++)
            {
                var wordCount = keys[i];
                string word = "";
                for(int j = i;j<textArray.Count();j++)
                {
                    if (wordCount > word.Count()) {
                        word = word + textArray[j];
                        j = j + nvalue;
                    }
                }
                words.Add(word);
            }

            return string.Join(" ", words);
        }
    }
}
