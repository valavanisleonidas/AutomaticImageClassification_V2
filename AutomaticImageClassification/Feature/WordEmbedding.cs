using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutomaticImageClassification.Utilities;

namespace AutomaticImageClassification.Feature
{
    public class WordEmbedding : IFeatures
    {

        //stopwords to be removed
        private List<string> _stopwords = new List<string>();

        private bool _removeStopwords;
        private bool _addZeroIfTermDoesNotExist;
        private bool _useTfidf;
        private Dictionary<string, double[]> _wordEmbeddings = new Dictionary<string, double[]>();
        private Dictionary<string, double> _wordIdf = new Dictionary<string, double>();
        private List<string[]> _alltermDocsArray = new List<string[]>();
        private int _embeddingsLength;

        public WordEmbedding()
        {
            _useTfidf = false;
            _addZeroIfTermDoesNotExist = false;
            _removeStopwords = false;
            _embeddingsLength = 0;

        }

        public WordEmbedding(Dictionary<string, double[]> wordEmbeddings, List<string[]> alltermDocsArray,
            int embeddingsLength, bool removeStopwords, bool addZeroIfTermDoesNotExist, bool useTfidf)
        {

            this._wordEmbeddings = wordEmbeddings;
            this._embeddingsLength = embeddingsLength;
            this._alltermDocsArray = alltermDocsArray;
            this._removeStopwords = removeStopwords;
            this._addZeroIfTermDoesNotExist = addZeroIfTermDoesNotExist;
            this._useTfidf = useTfidf;
        }


        public double[] ExtractHistogram(string input)
        {
            var featureVector = new double[_embeddingsLength];
            var docWordEmbeddings = new Dictionary<string, double[]>();

            //get terms of sentence
            var docTerms = Regex.Split(input.Replace(@"[\W&&[^\s]]", ""), @"\W+");
            
            foreach (var term in docTerms)
            {
                var termLower = term.ToLower();

                //if term exists already in document continue
                if (docWordEmbeddings.ContainsKey(termLower))
                    continue;

                //if term is stopword continue
                if (_removeStopwords && _stopwords.Contains(termLower))
                    continue;

                var termEmbedding = new double[_embeddingsLength];

                if (_wordEmbeddings.ContainsKey(termLower))
                {
                    //get embedding
                    _wordEmbeddings.TryGetValue(termLower, out termEmbedding);

                    var tf = Normalization.ComputeTf(docTerms, termLower);
                    var finalScore = tf;

                    if (_useTfidf)
                    {
                        double idf;
                        if (_wordIdf.ContainsKey(termLower))
                        {
                            _wordIdf.TryGetValue(termLower, out idf);
                        }
                        else
                        {
                            //calculate idf
                            idf = Normalization.ComputeIdf(_alltermDocsArray, termLower);
                            _wordIdf.Add(termLower, idf);
                        }
                        finalScore *= idf;
                    }
                    //multiply with idf
                    termEmbedding = Normalization.WeightArray(termEmbedding, finalScore);
                    docWordEmbeddings.Add(termLower, termEmbedding);
                }
                else
                {
                    if (!_addZeroIfTermDoesNotExist) continue;

                    //add zeros
                    termEmbedding = Enumerable.Repeat(0.0, termEmbedding.Length).ToArray();
                    docWordEmbeddings.Add(termLower, termEmbedding);
                }
            }
            //if no embeddings then add array with zeros else get centroid for embeddings
            featureVector = docWordEmbeddings.Count == 0 
                ? Enumerable.Repeat(0.0, featureVector.Length).ToArray() 
                : GetCentroid(docWordEmbeddings.Values.ToArray());
            
            return featureVector;
        }

        public List<double[]> ExtractDescriptors(string input)
        {
            throw new NotImplementedException();
        }


        //get sum of each column and divide by columns size
        private static double[] GetCentroid(IReadOnlyList<double[]> docEmbeddings)
        {
            var featuresSize = docEmbeddings[0].Length;
            var imagesSize = docEmbeddings.Count;
            var array = new double[featuresSize];

            for (var i = 0; i < featuresSize; i++)
            {
                double sum = 0;
                for (var j = 0; j < imagesSize; j++)
                {
                    sum += docEmbeddings[j][i];
                }
                array[i] = sum / imagesSize;
            }
            return array;
        }


    }

}
