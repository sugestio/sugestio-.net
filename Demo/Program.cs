using System;
using System.Collections.Generic;
using System.Text;
using Sugestio;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            //GetRecommendations();
            //GetSimilarItems();
            AddConsumption();
            Console.ReadLine();
        }

        static void GetRecommendations()
        {
            Client client = new Client("sandbox", "demo");
            List<Recommendation> recommendations = client.GetRecommendations("1");
            Console.WriteLine("Call complete, " + recommendations.Count + " recommendations.");
            recommendations.ForEach(Print);
        }

        static void GetSimilarItems()
        {
            Client client = new Client("sandbox", "demo");
            List<Recommendation> similarItems = client.GetSimilar("1");
            Console.WriteLine("Call complete, " + similarItems.Count + " similar items.");
            similarItems.ForEach(Print);
        }

        /// <summary>
        /// Posts a consumption to the debug endpoint and sets the optional consumption id to a specific value
        /// </summary>
        static void AddConsumption()
        {
            Client client = new Client("sandbox", "demo", true);
            Consumption consumption = new Consumption("123", "ABCD");
            consumption.Id = "IDX";
            int response = client.Add(consumption);
            Console.WriteLine("Call complete, status code: " + response);
        }

        static void Print(Recommendation r)
        {
            Console.WriteLine("Item " + r.ItemId);
        }
    }

}
