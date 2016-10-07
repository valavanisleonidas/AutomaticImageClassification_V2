using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutomaticImageClassification.Utilities;
using ikvm.extensions;

namespace AutomaticImageClassification.Feature
{
    public class TfIdf : IFeatures
    {
        private TfidfApproach _tfidf;

        public double[] ExtractHistogram(string input)
        {
            var tfidfvector = new double[_tfidf.AllTerms.Count];

            var tokenizedSentence = input.replaceAll("[\\W&&[^\\s]]", "").split("\\W+");   //to get individual terms

            //for each term
            foreach (var term in tokenizedSentence)
            {

                var termLower = term.ToLower();
                if (_tfidf.RemoveStopwords && _tfidf.Stopwords.Contains(termLower))
                    continue;

                //if term exists in all terms then continue else discard word
                var index = _tfidf.AllTerms.IndexOf(term);
                if (index == -1)
                {
                    //System.out.println("Word "+ term +" does not exist in sentence "+ docTermsArray.toString());
                    continue;
                }

                var tf = Normalization.ComputeTf(tokenizedSentence, term); //term frequency
                var finalScore = tf; //term frequency inverse document frequency   
                if (_tfidf.UseTfidf)
                {
                    double idf; //inverse document frequency
                    if (_tfidf.WordIdf.ContainsKey(term))
                    {
                        _tfidf.WordIdf.TryGetValue(term,out idf);
                    }
                    else
                    {
                        //calculate idf
                        idf = Normalization.ComputeIdf(_tfidf.TermsDocsArray, term);
                        _tfidf.WordIdf.Add(term, idf);
                    }
                    finalScore = tf * idf;
                }

                //System.out.print*/ln("term " + terms + " was FOUND in document: " + counter + " with TFIDF : " + tfidf + " tf :" + tf + " idf :" + idf);
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

        public Dictionary<string, double> WordIdf = new Dictionary<string, double>();

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
            TermsDocsArray = new List<string[]>();
            TfidfDocsVector = new List<double[]>();
            WordIdf = new Dictionary<string, double>();
            

            //each row contains one doc (image)    	
            foreach (var figurese in images)
            {
                string doc = figurese.Title + " " + figurese.Caption;
                string[] tokenizedTerms = doc.replaceAll("[\\W&&[^\\s]]", "").split("\\W+"); //to get individual terms

                TermsDocsArray.Add(tokenizedTerms);
                //if test dont add to allterms
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


    }

}
