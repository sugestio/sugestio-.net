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
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Sugestio
{
    public class Consumption : ISugestioObject
    {       
        private string userId = null;

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private string itemId = null;

        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        private string type = null;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string detail = null;

        public string Detail
        {
            get { return detail; }
            set { detail = value; }
        }

        private string date = "NOW";

        public string Date
        {
            get { return date; }
            set { date = value; }
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
            return "/consumptions";
        }

        public IDictionary ToDictionary()
        {

            var dictionary = new ListDictionary();            
            dictionary.Add("userid", userId);
            dictionary.Add("itemid", itemId);

            if (type != null)
                dictionary.Add("type", type);

            if (detail != null)
                dictionary.Add("detail", detail);

            if (date != null)
                dictionary.Add("date", date);

            return dictionary;
        }
    }
}
