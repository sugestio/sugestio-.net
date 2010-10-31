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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using System.Xml.Linq;

using DevDefined.OAuth;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace Sugestio
{
    public class Client
    {
        private string account;
        private string secret;

        private static readonly String baseUri = "http://api.sugestio.com/sites/";
        private OAuthConsumerContext consumerContext;
        private OAuthSession session;

        public Client() 
            : this("sandbox", "demo") 
        {
            
        }

        public Client(string account, string secret)
        {
            this.account = account;
            this.secret = secret;
            init();
        }

        private void init()
        {
            this.consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = account,
                SignatureMethod = SignatureMethod.HmacSha1,
                ConsumerSecret = secret
            };

            this.session = new OAuthSession(consumerContext, 
                "http://api.sugestio.com", 
                "http://api.sugestio.com", 
                "http://api.sugestio.com");
        }

        public List<Recommendation> getRecommendations(string userId)
        {
            return getRecommendedItems("recommendations", userId);
        }

        public List<Recommendation> getSimilar(string itemId)
        {
            return getRecommendedItems("similar", itemId);
        }

        private List<Recommendation> getRecommendedItems(string type, string id)
        {

            List<Recommendation> recommendations = new List<Recommendation>();
            string url = "";

            if (type.Equals("recommendations"))
            {
                url = getUri("/users/" + id + "/recommendations.xml");
            }
            else if (type.Equals("similar")) 
            {
                url = getUri("/items/" + id + "/similar.xml");
            }

            var request = session.Request().Get().ForUrl(url);

            XDocument loaded = doGet(request);

            if (loaded != null)
            {
                var data = from item in loaded.Descendants("recommendation")
                           select new Recommendation
                           {
                               ItemId = (string)item.Element("itemid") ?? null,
                               Score = Convert.ToDouble((string)item.Element("score") ?? "0.0"),
                               Algorithm = (string)item.Element("algorithm").Value ?? "",
                               Certainty = Convert.ToDouble((string)item.Element("certainty") ?? "0.0")
                           };

                foreach (var r in data)
                {
                    if (r.ItemId != null)
                    {
                        recommendations.Add(r);
                    }
                }
            }

            return recommendations;

        }

        private XDocument doGet(IConsumerRequest request)
        {

            try
            {
                return request.ToDocument();
            }
            catch (OAuthException ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    //Console.WriteLine("Status Code : {0}", response.StatusCode);
                    //Console.WriteLine("Status Description : {0}", response.StatusDescription);

                }
                else
                {
                    Console.Write(ex.Message);
                }

                return null;

            }

        }

        private string getUri(string resource)
        {
            return baseUri + account + resource;
        } 

    }
}
