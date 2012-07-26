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
using System.Collections;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace Sugestio
{
    [XmlType("item")]
    public class Item : ISugestioObject
    {

        private string id = null;
        [XmlElement("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string title = null;
        [XmlElement("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string available = null;
        [XmlElement("available")]
        public String Available
        {
            get { return available; }
            set { available = value; }
        }

        private string description_short = null;
        [XmlElement("description_short")]
        public String Description_short
        {
            get { return description_short; }
            set { description_short = value; }
        }

        private string description_long = null;
        [XmlElement("description_long")]
        public String Description_long
        {
            get { return description_long; }
            set { description_long = value; }
        }

        private string from = null;
        [XmlElement("from")]
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        private string until = null;
        [XmlElement("until")]
        public string Until
        {
            get { return until; }
            set { until = value; }
        }

        private string location_city = null;
        [XmlElement("location_city")]
        public string Location_city
        {
            get { return location_city; }
            set { location_city = value; }
        }

        private string location_simple = null;
        [XmlElement("location_simple")]
        public string Location_simple
        {
            get { return location_simple; }
            set { location_simple = value; }
        }

        private string location_latlong = null;
        [XmlElement("location_latlong")]
        public string Location_latlong
        {
            get { return location_latlong; }
            set { location_latlong = value; }
        }
        
        private IList categories = new ArrayList();
        [XmlElement("category")]
        public IList Categories
        {
            get { return categories; }
            set { categories = value; }
        }

        private IList creators = new ArrayList();
        [XmlElement("creator")]
        public IList Creators
        {
            get { return creators; }
            set { creators = value; }
        }

        private IList segments = new ArrayList();
        [XmlElement("segment")]
        public IList Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        private IList tags = new ArrayList();
        [XmlElement("tag")]
        public IList Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        private string permalink = null;
        [XmlElement("permalink")]
        public string Permalink
        {
            get { return permalink; }
            set { permalink = value; }
        }

        private string thumbnail = null;
        [XmlElement("thumbnail")]
        public string Thumbnail
        {
            get { return thumbnail; }
            set { thumbnail = value; }
        }

        public Item()
        {
        }

        public Item(string id)
        {
            this.id = id;
        }

        public string GetResource()
        {
            return "/items.xml";
        }

        public string getRootName()
        {
            return "items";
        }
    }
}
