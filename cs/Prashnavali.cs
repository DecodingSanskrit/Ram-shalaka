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
        //char[] charSet = new char[] { 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','`','~','!','@','#','$','%','^','&','(',')','_','{','}','[',']','|','\\',':',';','"','.','?',',','+','-','*','/','<','>','=','\'','0','1','2','3','4','5','6','7','8','9' };

        List<char> charSet = new List<char>();
        //Updated 148 Character Set
        string sre = "!\"#$%&()*+'-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~€‚ƒ„…†‡ˆ‰Š‹ŒŽ“”•–—˜™š›œžŸ¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿";

        public List<int> Key;
        public List<int> Key2;
        public int select = 6;

        public Prashnavali()
        {
            charSet = sre.ToCharArray().ToList();
        }

        public Prashnavali(List<int> key1,List<int> key2,int select)
        {
            Key = key1;
            Key2 = key2;
            this.select = select;
        }

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
            //select = random.Next(avg, wordsCount);
            var square = Convert.ToInt32(nearest_sq);
            List<char> TransposedText = new List<char>();
            var range = new char[square];
            TransposedText.InsertRange(0, range);
            for (int j=0;j<words.Count();j++)
            {
                var wordCharArray = words[j].ToCharArray();
                var wordCount = j;
                if (TransposedText[wordCount] != '\0')
                {
                    wordCount = TransposedText.IndexOf('\0');
                }
                
                for (int i = 0; i < wordCharArray.Count();i++)
                {
                    if(wordCount >= TransposedText.Count())
                    {
                        TransposedText.AddRange(new char[select]);
                    }
                    if (TransposedText[wordCount] != '\0')
                    {

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
                var text = TransposedText[i];
                var data = charSet.IndexOf(text);
                //var data = Array.IndexOf(charSet, (int)text);
                
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
            List<int> intArray = new List<int>();
            List<bool> checkArray = new List<bool>();
            
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
                var data = charSet.IndexOf(charArray[i]);
                //var data = Array.IndexOf(charSet, charArray[i]);
                data = data + charSet.Count();
                var result = data - Key[i];
                while(result < 0)
                {
                    result = result + charSet.Count();
                }
                var intresult = data - Key[i];
                //var mod = Math.Abs(result) % 68;
                intArray.Add(intresult);
                textArray.Add(charSet[result]);
                checkArray.Add(false);
            }

            List<string> words = new List<string>();

            //var sub = textArray.Count() % root;
            var height = Math.Round(textArray.Count() / root);
            

            var hw = height + root;
            var colrow = textArray.Count() + count;
            var col = (colrow - hw);
            var nvalue  = (int)(col / hw);
            //nvalue = keys.Count() * nvalue;
            //var select = textArray.Count() % avg;

            for(int i = 0; i < keys.Count(); i++)
            {
                var wordCount = keys[i];
                string word = "";
                var index = i;
                if (checkArray[i] == true)
                {
                    index = checkArray.IndexOf(false);

                }

                for(int j = index;j<textArray.Count();)
                {
                    if (wordCount > word.Count()) {
                        word = word + textArray[j];
                        checkArray[j] = true;
                        j = j + select;
                    }
                    else
                    {
                        break;
                    }
                }
                words.Add(word);
            }

            return string.Join(" ", words);
        }
    }
}
