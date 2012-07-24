# Overview

This is a .NET library for interfacing with the [Sugestio](http://www.sugestio.com) 
recommendation service. Data is submitted as XML. The library uses 
[DevDefined.OAuth](http://github.com/bittercoder/DevDefined.OAuth) to create the OAuth-signed requests. Written in C# for .NET Framework 3.5.

## About Sugestio

Sugestio is a scalable and fault tolerant service that now brings the power of 
web personalisation to all developers. The RESTful web service provides an easy to use 
interface and a set of developer libraries that enable you to enrich 
your content portals, e-commerce sites and other content based websites.

### Access credentials and the Sandbox

To access the Sugestio service, you need an account name and a secret key. 
To run the examples from the tutorial, you can use the following credentials:

* account name: <code>sandbox</code>
* secret key: <code>demo</code>

The Sandbox is a *read-only* account. You can use these credentials to experiment 
with the service. The Sandbox can give personal recommendations for users 1 through 5, 
and similar items for items 1 through 5.

When you are ready to work with real data, you may apply for a developer account through 
the [Sugestio website](http://www.sugestio.com).  

## About this library

### Features

The following [API](http://www.sugestio.com/documentation) features are implemented:

* get personalized recommendations for a given user
* get items that are similar to a given item
* submit user activity (consumptions): clicks, purchases, ratings, ...
* submit item metadata: description, location, tags, categories, ...

### Requirements

* .NET Framework 3.5 or newer
* foretagsplatsen's [fork](http://github.com/foretagsplatsen/DevDefined.OAuth) of DevDefined.OAuth until their changes get merged

# Tutorial and sample code

<code>SugestioDemo/Program.cs</code> contains more sample code that illustrates how you can use the library.

## Getting recommendations
The following example gets personal recommendations for user 1:

### Code

	using Sugestio;

	static void GetRecommendations()
	{
		Client client = new Client("sandbox", "demo"); // account, secret
        List<Recommendation> recommendations = client.GetRecommendations("1");
        Console.WriteLine("Call complete, " + recommendations.Count + " recommendations.");
        recommendations.ForEach(Print);
	}

	static void Print(Recommendation r)
	{
		Console.WriteLine("Item " + r.ItemId);
	}

### Response

	Call complete, 5 recommendations.
	Item 1
	Item 2
	Item 3
	Item 4
	Item 5	

## Submitting consumptions
This example submits a consumption to the debug endpoint. We also set the consumption id to a specific value rather than 
having Sugestio auto-generate a UUID identifier for us.

### Code

	using Sugestio;

	static void AddConsumption()
    {
		Client client = new Client("sandbox", "demo", true); // account, secret, use debug endpoint
        Consumption consumption = new Consumption("123", "ABCD"); // userid and itemid are required
        consumption.Id = "X"; // optional, a UUID will be assigned if not set manually
        int response = client.Add(consumption);
        Console.WriteLine("Call complete, status code: " + response);
    }

### Response

	Call complete, status code: 202

## Submitting item metadata
This example illustrates how to submit item metadata. Values are assigned to both scalar (title) and 
non-scalar (category, creator) attributes.

### Code

	using Sugestio;

	static void AddConsumption()
    {
		Client client = new Client("sandbox", "demo", true); // account, secret, use debug endpoint
        Item item = new Item("X"); // itemid is required
        item.Title = "Item X";            
        item.Categories.Add("Category A");
        item.Categories.Add("Category B");
        item.Creators.Add("Artist A");
        int response = client.Add(item);
        Console.WriteLine("Call complete, status code: " + response);
    }

### Response

	Call complete, status code: 202