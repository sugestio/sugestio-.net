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
using System.Xml;
using System.IO;

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
            //send data as XML using foretagsplatsen's fork (see OAuthRestClient.cs)
            string url = GetUrl(sugestioObject.GetResource());
            string body = getBody(sugestioObject);
            //Console.WriteLine(body);
            return Post(url, body);
        }

        /// <summary>
        /// Bulk add consumptions
        /// </summary>
        /// <param name="consumptions">the consumptions to add</param>
        /// <returns>HTTP status code</returns>
        public int Add(List<Consumption> consumptions)            
        {
            return AddGeneric(consumptions);            
        }

        /// <summary>
        /// Bulk add items
        /// </summary>
        /// <param name="items">the items to add</param>
        /// <returns>HTTP status code</returns>
        public int Add(List<Item> items)
        {
            return AddGeneric(items);
        }

        /// <summary>
        /// Bulk add users
        /// </summary>
        /// <param name="users">the users to add</param>
        /// <returns>HTTP status code</returns>
        public int Add(List<User> users)
        {
            return AddGeneric(users);
        }

        private int AddGeneric<T>(List<T> sugestioObjects)
            where T : ISugestioObject
        {
            if (sugestioObjects == null || sugestioObjects.Count == 0)
            {
                return 400;
            }
            
            //send data as XML using foretagsplatsen's fork (see OAuthRestClient.cs)
            string url = GetUrl(sugestioObjects.ElementAt(0).GetResource());
            string rootName = sugestioObjects.ElementAt(0).getRootName();
            string body = getBody(sugestioObjects, rootName);
            //Console.WriteLine(body);
            return Post(url, body);
        }

        private string getBody(ISugestioObject sugestioObject)
        {
            var serializer = new XmlSerializer(sugestioObject.GetType());
            var sw = new System.IO.StringWriter();
            serializer.Serialize(sw, sugestioObject);
            return sw.ToString();
        }

        private string getBody<T>(List<T> sugestioObjects, string rootName) where T : ISugestioObject
        {
            var sw = new System.IO.StringWriter();            
            var root = new XmlRootAttribute(rootName);
            var serializer = new XmlSerializer(sugestioObjects.GetType(), root);
            serializer.Serialize(sw, sugestioObjects);
            return sw.ToString();
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
            return parseRecommendedItems(Get(request));
        }

        /// <summary>
        /// Performs a POST request and returns the HTTP status code of the response.
        /// 202: Accepted
        /// 400: Bad Request
        /// 401: Unauthorized
        /// 500: Internal Server Error
        /// </summary>
        /// <param name="url">the url</param>
        /// <param name="body">the post body</param>
        /// <returns>HTTP Status Code</returns>
        private int Post(string url, string body)
        {
            try
            {
                var request = session.Request().Post().ForUrl(url);
                request.AlterContext(context => context.UseQueryParametersForOAuth = true);
                request.AlterHttpWebRequest(httpRequest => httpRequest.ContentType = "text/xml");
                request.ConsumerContext.EncodeRequestBody = false;
                request.WithBody(body);
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
        /// Parses a response body and returns a list of recommendations. An empty list is returned if there was an error.
        /// </summary>
        /// <param name="body">the response body</param>
        /// <returns>list of recommendations</returns>
        private List<Recommendation> parseRecommendedItems(string body)
        {
            if (body == null)
                return new List<Recommendation>();

            try
            {
                XmlReader reader = XmlReader.Create(new StringReader(body));
                XmlRootAttribute root = new XmlRootAttribute("recommendations");
                var serializer = new XmlSerializer(typeof(List<Recommendation>), root);
                return (List<Recommendation>)serializer.Deserialize(reader); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new List<Recommendation>();
            }
                          
        }

        /// <summary>
        /// Performs the GET request and returns the response body.
        /// If there was a problem (HTTP Status Code >= 400), null is returned.
        /// </summary>
        /// <param name="request">the response body</param>
        /// <returns>list of recommendations</returns>
        private string Get(IConsumerRequest request)
        {

            try
            {
                return request.ToString();                
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
