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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

using DevDefined.OAuth;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace Sugestio
{
    public class Client
    {
        private string account;
        private string secret;

        private Boolean debug = false;

        private static readonly String debugUrl = "http://debug.sugestio.com/sites/";
        private static readonly String baseUrl = "http://api.sugestio.com/sites/";
        private OAuthConsumerContext consumerContext;
        private OAuthSession session;

        /// <summary>
        /// Creates an instance of the Sugestio client with the default (Sandbox) credentials
        /// </summary>
        public Client() 
            : this("sandbox", "demo") 
        {
            
        }

        /// <summary>
        /// Creates an instance of the Sugestio client with the given access credentials
        /// </summary>
        /// <param name="account">your account name</param>
        /// <param name="secret">your secret key</param>
        public Client(string account, string secret) 
            : this(account, secret, false)
        {
            
        }

        /// <summary>
        /// Creates an instance of the Sugestio client with the given access credentials with choice of standard or debug endpoint
        /// </summary>
        /// <param name="account">your account name</param>
        /// <param name="secret">your secret key</param>
        /// <param name="debug">use debug endpoint?</param>
        public Client(string account, string secret, Boolean debug)
        {
            this.account = account;
            this.secret = secret;
            this.debug = debug;
            Init();
        }


        /// <summary>
        /// Initializes an OAuth session object which will be reused for all
        /// subsequent service calls
        /// </summary>
        private void Init()
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

        /// <summary>
        /// Gets personal recommendations for the given user.
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>list of recommendations</returns>
        public List<Recommendation> GetRecommendations(string userId)
        {
            return GetRecommendedItems("recommendations", userId);
        }

        /// <summary>
        /// Gets items that are similar to the given item.
        /// </summary>
        /// <param name="itemId">id of the item</param>
        /// <returns>list of similar items</returns>
        public List<Recommendation> GetSimilar(string itemId)
        {
            return GetRecommendedItems("similar", itemId);
        }

        /// <summary>
        /// Adds a consumption, a user or an item
        /// </summary>
        /// <param name="sugestioObject">the entity to add</param>
        /// <returns>HTTP status code</returns>
        public int Add(ISugestioObject sugestioObject)
        {
            string url = GetUrl(sugestioObject.GetResource());
            var request = session.Request().Post().ForUrl(url);
            //request.WithFormParameters(sugestioObject.ToDictionary());
            //send data as XML using foretagsplatsen's fork (see OAuthRestClient.cs)
            var serializer = new XmlSerializer(sugestioObject.GetType());
            var sw = new System.IO.StringWriter();                
            serializer.Serialize(sw, sugestioObject);
            string body = sw.ToString();
            
            request.AlterContext(context => context.UseQueryParametersForOAuth = true);
            request.AlterHttpWebRequest(httpRequest => httpRequest.ContentType = "text/xml");
            request.ConsumerContext.EncodeRequestBody = false;
            request.WithBody(body);
            //Console.WriteLine(body);                                                          
            return Post(request);
        }        

        /// <summary>
        /// Performs a GET request for recommendations or similar items and
        /// turns the XML response body into a list of recommendations.
        /// </summary>
        /// <param name="type">type of resource to get ("recommendations" or "similar")</param>
        /// <param name="id">id of the user or item</param>
        /// <returns>list of recommendations or similar items</returns>
        private List<Recommendation> GetRecommendedItems(string type, string id)
        {

            List<Recommendation> recommendations = new List<Recommendation>();
            string url = "";

            if (type.Equals("recommendations"))
            {
                url = GetUrl("/users/" + id + "/recommendations.xml");
            }
            else if (type.Equals("similar")) 
            {
                url = GetUrl("/items/" + id + "/similar.xml");
            }

            var request = session.Request().Get().ForUrl(url);

            XDocument loaded = Get(request);

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

        /// <summary>
        /// Performs a POST request and returns the HTTP status code of the response.
        /// 202: Accepted
        /// 400: Bad Request
        /// 401: Unauthorized
        /// 500: Internal Server Error
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>HTTP Status Code</returns>
        private int Post(IConsumerRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.ToWebResponse();
                return Convert.ToInt32(response.StatusCode);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    return Convert.ToInt32(response.StatusCode); 
                }
                else
                {
                    Console.Write(ex.Message);
                    return 0;
                }
            }
        }

        /// <summary>
        /// Performs the GET request and returns the XDocument from the response body.
        /// If there was a problem (HTTP Status Code >= 400), null is returned.
        /// </summary>
        /// <param name="request">the request</param>
        /// <returns>XDocument from response body or null</returns>
        private XDocument Get(IConsumerRequest request)
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

        /// <summary>
        /// Builds the full URL for the resource.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private string GetUrl(string resource)
        {
            if (!debug)
                return baseUrl + account + resource;
            else
                return debugUrl + account + resource;
        } 

    }
}
