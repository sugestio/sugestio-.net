﻿#region License

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
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace Sugestio
{
    [XmlType("consumption")] 
    public class Consumption : ISugestioObject
    {

        private string id = null;
        [XmlElement("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string userId = null;
        [XmlElement("userid")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string itemId = null;
        [XmlElement("itemid")]
        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }
        
        private string type = null;
        [XmlElement("type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string detail = null;
        [XmlElement("detail")]
        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        private string date = "NOW";
        [XmlElement("date")]
        public string Date
        {
            get { return date; }
            set { date = value; }
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

        public Consumption()
        {
        }

        public Consumption(string userId, string itemId) 
            : this(userId, itemId, null, null)
        {

        }

        public Consumption(string userId, string itemId, string type, string detail)
        {
            this.userId = userId;
            this.itemId = itemId;
            this.type = type;
            this.detail = detail;
        }

        public string GetResource()
        {
            return "/consumptions.xml";
        }

        public string getRootName()
        {
            return "consumptions";
        }
        
    }
}
