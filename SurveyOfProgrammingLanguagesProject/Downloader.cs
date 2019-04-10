using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SurveyOfProgrammingLanguagesProject
{
    public class Downloader
    {
        private WebClient downloader;
        private const string APIKey = "aa7cc97ce99e4b07b48eafb6161af307";

        private string _word;
        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                _word = value;
            }
        }

        public Downloader(string word)
        {
            this.Word = word;
            downloader = new WebClient();
        }

        public void DownloadSynonyms()
        {
            try
            {
                downloader.DownloadFile("http://words.bighugelabs.com/api/2/" + APIKey + "/" + Word + "/xml", Word + ".xml");
            }
            catch(ArgumentNullException n)
            {
                Console.WriteLine("A word must be passed in.");
            }
            catch(WebException w)
            {
                Console.WriteLine("No connection or no synonyms found.\n" + w.Message + "Word= " + Word);
            }
            catch(NotSupportedException n)
            {
                Console.WriteLine("You must be able to connect to the internet.");
            }
           

        }


    }
}
