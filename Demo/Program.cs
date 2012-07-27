using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sugestio;

namespace Demo
{
    class Program
    {
        static string account = "sandbox";
        static string secret = "demo";
        static Boolean debug = true; // use debug endpoint?

        static void Main(string[] args)
        {
            //GetRecommendations();
            //GetSimilarItems();
            //AddConsumption();
            //BulkAddConsumptions();
            //AddItem();
            //BulkAddItems();
            //AddUser();
            //BulkAddUsers();
            Console.ReadLine();
        }

        /// <summary>
        /// Gets personal recommendations for a given user
        /// </summary>
        static void GetRecommendations()
        {
            Client client = new Client(account, secret);
            List<Recommendation> recommendations = client.GetRecommendations("1");
            Console.WriteLine("Call complete, " + recommendations.Count + " recommendations.");
            recommendations.ForEach(Print);
        }

        /// <summary>
        /// Gets items that are similar to a given item
        /// </summary>
        static void GetSimilarItems()
        {
            Client client = new Client(account, secret);
            List<Recommendation> similarItems = client.GetSimilar("1");
            Console.WriteLine("Call complete, " + similarItems.Count + " similar items.");
            similarItems.ForEach(Print);
        }

        /// <summary>
        /// Prints a recommendation
        /// </summary>
        /// <param name="recommendation">the recommendation</param>
        static void Print(Recommendation recommendation)
        {
            Console.WriteLine(recommendation.Item.Title);
        }

        /// <summary>
        /// Posts a consumption and sets the optional consumption id to a specific value
        /// </summary>
        static void AddConsumption()
        {
            Client client = new Client(account, secret, debug);
            Consumption consumption = new Consumption("123", "ABCD");
            consumption.Id = "IDX";
            int response = client.Add(consumption);
            Console.WriteLine("Call complete, status code: " + response);
        }

        /// <summary>
        /// Bulk posts consumptions
        /// </summary>
        static void BulkAddConsumptions()
        {
            Client client = new Client(account, secret, debug);
            List<Consumption> consumptions = new List<Consumption>();

            Consumption c1 = new Consumption("1", "A");
            consumptions.Add(c1);

            Consumption c2 = new Consumption("2", "B");            
            consumptions.Add(c2);
            
            int response = client.Add(consumptions);            
            Console.WriteLine("Call complete, status code: " + response);
        }

        /// <summary>
        /// Posts item metadata
        /// </summary>
        static void AddItem()
        {
            Client client = new Client(account, secret, debug);
            Item item = new Item("X");
            item.Title = "Item X";            
            item.Categories.Add("Category A");
            item.Categories.Add("Category B");
            item.Creators.Add("Artist A");
            int response = client.Add(item);
            Console.WriteLine("Call complete, status code: " + response);
        }

        /// <summary>
        /// Bulk posts item metadata
        /// </summary>
        static void BulkAddItems()
        {
            Client client = new Client(account, secret, debug);
            List<Item> items = new List<Item>();
            
            Item item1 = new Item("1");
            item1.Title = "Item 1";
            item1.Categories.Add("Category A");
            item1.Categories.Add("Category B");
            items.Add(item1);

            Item item2 = new Item("2");
            item2.Title = "Item 2";
            item2.Categories.Add("Category B");
            item2.Categories.Add("Category C");
            items.Add(item2);

            int response = client.Add(items);
            Console.WriteLine("Call complete, status code: " + response);
        }

        /// <summary>
        /// Posts user metadata
        /// </summary>
        static void AddUser()
        {
            Client client = new Client(account, secret, debug);
            User user = new User("X");
            user.Gender = "M";
            user.Birthday = "1960-07-26";
            int response = client.Add(user);
            Console.WriteLine("Call complete, status code: " + response);
        }

        /// <summary>
        /// Bulk posts user metadata
        /// </summary>
        static void BulkAddUsers()
        {
            Client client = new Client(account, secret, debug);
            List<User> users = new List<User>();

            User user1 = new User("1");
            user1.Gender = "M";
            user1.Birthday = "1960-07-26";
            users.Add(user1);

            User user2 = new User("2");
            user2.Gender = "F";
            user2.Birthday = "1962-06-10";
            users.Add(user2);

            int response = client.Add(users);
            Console.WriteLine("Call complete, status code: " + response);
        }
    }

}

