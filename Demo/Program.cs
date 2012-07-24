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
            //AddConsumption();
            //AddItem();
            Console.ReadLine();
        }

        /// <summary>
        /// Gets personal recommendations for a given user
        /// </summary>
        static void GetRecommendations()
        {
            Client client = new Client("sandbox", "demo");
            List<Recommendation> recommendations = client.GetRecommendations("1");
            Console.WriteLine("Call complete, " + recommendations.Count + " recommendations.");
            recommendations.ForEach(Print);
        }

        /// <summary>
        /// Gets items that are similar to a given item
        /// </summary>
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

        /// <summary>
        /// Posts item metadata to the debug endpoint
        /// </summary>
        static void AddItem()
        {
            Client client = new Client("sandbox", "demo", true);
            Item item = new Item("X");
            item.Title = "Item X";            
            item.Categories.Add("Category A");
            item.Categories.Add("Category B");
            item.Creators.Add("Artist A");
            int response = client.Add(item);
            Console.WriteLine("Call complete, status code: " + response);
        }

        static void Print(Recommendation r)
        {
            Console.WriteLine("Item " + r.ItemId);
        }
    }

}
