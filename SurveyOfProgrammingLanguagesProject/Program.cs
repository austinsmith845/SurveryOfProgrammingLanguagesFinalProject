using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace SurveyOfProgrammingLanguagesProject
{
    class Program
    {


        static Dictionary<string,words> cache = new Dictionary<string,words>();

        static void Main(string[] args)
        {
            bool run = true;
            LoadCache(cache);

            while (run)
            {
                Console.WriteLine("Enter the path of the file whose words you'd like to replace:");
                string file = Console.ReadLine();


                ReplaceWords(file);

                Console.WriteLine("Complete");
                Console.WriteLine("Would you like to run the program again? 0 = no, any other number =  yes");
                int again = int.Parse(Console.ReadLine());
                if(again != 0)
                {
                    run = true;
                }
                else
                {
                    run = false;
                }

                
            }

            
        }

        private static words GetWords(string word)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(words));
            try
            {
                StreamReader reader = new StreamReader(word + ".xml");
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "words";
                //reader.ReadToEnd();

                words words = (words)serializer.Deserialize(reader);
                reader.Close();
                return words;
            }
            catch(Exception e)
            {
                words w = new words();
                w.w = new wordsW[1];
                w.w[0]= new wordsW() { Value = word };
                return w;
            }


          
        }

        private static void AddSynonyms(words word,Word syn)
        {
            foreach(wordsW word1 in word.w)
            {
                syn.Synonyms.Add(word1.Value);
            }
        }

        /// <summary>
        /// This method loads the cache upon application start.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="syn"></param>
        private static void LoadCache(Dictionary<string,words> cache)
        {
            string[] files = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            IEnumerable<string> wordFiles = from string s in files where s.Contains(".xml") select s;

            foreach (string str in wordFiles)
            {
                FileInfo info = new FileInfo(str);
                string name = info.Name.Remove(info.Name.IndexOf(".xml"));
                int index = str.IndexOf(".xml");

                string s = str.Remove(index);

                if (!cache.ContainsKey(name))
                {
                    cache.Add(name.ToLower(), GetWords(s));
                }
                
             }

           
        }

        private static void ReplaceWords(string file)
        {
            StreamReader reader = new StreamReader(file);
            List<string> wordsToReplace = new List<string>();
            List<string> lines = new List<string>();
            char[] delimiterChars = { ' ', ',', '.', ':', '!','?', '\t' };

            while (!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }

            for(int i =0; i< lines.Count; i++)
            {
                string[] w = lines[i].Split(delimiterChars,StringSplitOptions.RemoveEmptyEntries);
                foreach(string s in w)
                {
                    wordsToReplace.Add(s);
                }
            }
            reader.Close();

            Random rand = new Random();
            StreamWriter writer = new StreamWriter(file);

            for (int i = 0; i < wordsToReplace.Count; i++)
            {
                words _words = PullFromCache(wordsToReplace[i]);
                Word syns = new Word();
                syns.Synonyms = new List<string>();
                if (_words == null)
                {
                    Console.WriteLine("Downloading synonyms.");
                    Downloader downloader = new Downloader(wordsToReplace[i]);
                    downloader.DownloadSynonyms();

                  
                     _words = GetWords(wordsToReplace[i]);
                    cache.Add(wordsToReplace[i].ToLower(), _words);
                }
                AddSynonyms(_words, syns);
                wordsToReplace[i]= syns.Synonyms[rand.Next(0, syns.Synonyms.Count)];
                writer.Write(wordsToReplace[i]+" ");
            }
            writer.Close();
         

        }

        /// <summary>
        /// Looks through the cache for the word needed to try and prevent multiple 
        /// downloads for the same word.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static words PullFromCache(string word)
        {
            try
            {
                words s = cache[word.ToLower()];
                Console.WriteLine("pulled from cache.");

                return s;
            }
            catch(Exception e)
            {
                return null;
            }
         
        }

       
    }
}
