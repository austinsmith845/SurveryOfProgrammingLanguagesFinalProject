using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace SurveyOfProgrammingLanguagesProject
{
    public class Word
    {
        private string _word;
        
        public string Word1
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

        private List<string> _synonyms;
        public List<string> Synonyms
        {
            get
            {
                return _synonyms;
            }

            set { _synonyms =  value; }

        }

    }
}
