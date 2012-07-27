#region License

// The MIT License
//
// Copyright (c) 2010 Sugestio
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Sugestio
{
    [XmlType("recommendation")]    
    public class Recommendation
    {
        private string itemId;
        [XmlElement("itemid")]
        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        private double score;
        [XmlElement("score")]
        public double Score
        {
            get { return score; }
            set { score = value; }
        }

        private double certainty;
        [XmlElement("certainty")]
        public double Certainty
        {
            get { return certainty; }
            set { certainty = value; }
        }

        private string algorithm;
        [XmlElement("algorithm")]
        public string Algorithm
        {
            get { return algorithm; }
            set { algorithm = value; }
        }

        private Item item;
        [XmlElement("item")]
        public Item Item
        {
            get { return item; }
            set { item = value; }
        }

        public Recommendation()
        {
        }

        public Recommendation(string itemId, double score, double certainty, string algorithm)
        {
            this.itemId = itemId;
            this.score = score;
            this.certainty = certainty;
            this.algorithm = algorithm;
        }

        
    }
}
