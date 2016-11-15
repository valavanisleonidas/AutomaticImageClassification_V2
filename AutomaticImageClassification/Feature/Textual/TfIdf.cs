using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature.Textual
{
    public class TfIdf : IFeatures
    {
        private TfidfApproach _tfidf;
        private Dictionary<string, double> _wordIdf = new Dictionary<string, double>();

        public TfIdf() { }

        public TfIdf(TfidfApproach tfidfApproach)
        {
            _tfidf = tfidfApproach;
        }

        public double[] ExtractHistogram(string input)
        {
            var tfidfvector = new double[_tfidf.AllTerms.Count];

            //get terms of sentence
            var tokenizedSentence = Regex.Split(input.Replace(@"[\W&&[^\s]]", ""), @"\W+");

            //for each term
            foreach (var term in tokenizedSentence)
            {

                var termLower = term.ToLower();
                if (_tfidf.RemoveStopwords && _tfidf.Stopwords.Contains(termLower))
                    continue;

                //if term exists in all terms then continue else discard word
                var index = _tfidf.AllTerms.IndexOf(termLower);
                if (index == -1)
                {
                    //System.out.println("Word "+ term +" does not exist in sentence "+ docTermsArray.toString());
                    continue;
                }

                var tf = Normalization.ComputeTf(tokenizedSentence, termLower); //term frequency
                var finalScore = tf; //term frequency inverse document frequency   
                if (_tfidf.UseTfidf)
                {
                    double idf; //inverse document frequency
                    if (_wordIdf.ContainsKey(termLower))
                    {
                        //get from dictionary
                        _wordIdf.TryGetValue(termLower, out idf);
                    }
                    else
                    {
                        //calculate idf and add to dictionary
                        idf = Normalization.ComputeIdf(_tfidf.TermsDocsArray, termLower);
                        _wordIdf.Add(termLower, idf);
                    }
                    finalScore = tf * idf;
                }
                tfidfvector[index] = finalScore;
            }
            return tfidfvector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            throw new NotImplementedException();
        }
    }


    public class TfidfApproach
    {
        //This variable will hold all terms of each document in an array.
        public List<string[]> TermsDocsArray = new List<string[]>();
        //to hold all terms
        public List<string> AllTerms = new List<string>();
        //tf-idf vector for each document
        public List<double[]> TfidfDocsVector = new List<double[]>();
        //stopwords to be removed
        public List<string> Stopwords = new List<string>();


        public bool RemoveStopwords = true;
        public bool UseTfidf = true;

        public TfidfApproach() { }

        public TfidfApproach(bool removeStopwords, bool useTfidf)
        {
            this.RemoveStopwords = removeStopwords;
            this.UseTfidf = useTfidf;
        }

        public void ParseData(List<Figure> images, bool isTrainSet)
        {

            //each row contains one doc (image)    	
            foreach (var figurese in images)
            {
                //var doc = figurese.Title + " " + figurese.Caption;
                var doc = figurese.Caption;

                //get terms of sentence
                var tokenizedTerms = Regex.Split(doc.Replace(@"[\W&&[^\s]]", ""), @"\W+");

                TermsDocsArray.Add(tokenizedTerms);
                //if test, dont add terms to allterms array
                if (!isTrainSet)
                    continue;

                foreach (var term in tokenizedTerms)
                {
                    var termLower = term.ToLower();

                    if (AllTerms.Contains(termLower)) //avoid duplicate entry
                        continue;

                    //avoid stopwords
                    if (RemoveStopwords && Stopwords.Contains(termLower))
                        continue;

                    AllTerms.Add(termLower);
                }
            }
        }

        public override string ToString()
        {
            return "TfIdf";
        }

    }

}
