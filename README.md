# Overview

This is a C# library for interfacing with the [Sugestio](http://www.sugestio.com) 
recommendation service. Data is submitted as XML. The library uses 
[DevDefined.OAuth](http://github.com/bittercoder/DevDefined.OAuth) to create the OAuth-signed requests. 

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

### Requirements

* .NET Framework 3.5 or newer
* foretagsplatsen's [fork](http://github.com/foretagsplatsen/DevDefined.OAuth) of DevDefined.OAuth until their changes get merged

# Tutorial and sample code

<code>Example.cs</code> contains sample code that illustrates how you can use the library.
The following example gets personal recommendations for user 1:

### Code

	using Sugestio;

	static void GetRecommendations()
	{
		Client client = new Client("sandbox", "demo");
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