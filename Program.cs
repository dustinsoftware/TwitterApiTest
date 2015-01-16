using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

public class Program
{
    public static void Main(string[] args)
    {
        var client = new RestClient("https://api.twitter.com/");
        var settings = JsonConvert.DeserializeObject<OAuthSettings>(File.ReadAllText("settings.json"));

        if (settings?.ConsumerToken == null)
            throw new InvalidOperationException("An invalid consumer token was specified.");
        if (settings?.ConsumerSecret == null)
            throw new InvalidOperationException("An invalid consumer secret was specified.");

        string bearer;

        var authRequest = new RestRequest("oauth2/token", Method.POST);
        client.Authenticator = new HttpBasicAuthenticator(settings.ConsumerToken, settings.ConsumerSecret);
        authRequest.AddParameter("grant_type", "client_credentials");
        var authResponse = client.Execute(authRequest);

        if (authResponse.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException("A token was not obtained successfully. Response: \{authResponse.Content}");

        var token = JsonConvert.DeserializeObject<OAuthToken>(authResponse.Content);

        client.Authenticator = null;

        var searchRequest = new RestRequest("1.1/search/tweets.json");
        searchRequest.AddParameter("q", args.Length == 0 ? "asp.net vNext" : args[0]);
        searchRequest.AddParameter("Authorization", "Bearer \{token.AccessToken}", ParameterType.HttpHeader);
        var searchResponse = client.Execute<TwitterSearchResponse>(searchRequest);

        if (searchResponse.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException("An obtained token was rejected. Response: \{searchResponse.Content}");

        foreach (var tweet in searchResponse?.Data?.statuses)
        {
            Console.WriteLine("Tweet: \{tweet.text}");
        }
    }

    private sealed class OAuthSettings
    {
        public OAuthSettings(string consumerToken, string consumerSecret)
        {
            ConsumerToken = consumerToken;
            ConsumerSecret = consumerSecret;
        }

        public string ConsumerToken { get; }
        public string ConsumerSecret { get; }
    }

    private sealed class OAuthToken
    {
        public OAuthToken(string token_type, string access_token)
        {
            TokenType = token_type;
            AccessToken = access_token;
        }

        public string TokenType { get; }
        public string AccessToken { get; }
    }

    private sealed class TwitterSearchResponse
    {
        public List<Status> statuses { get; set; }

        public class Status
        {
            public string text { get; set; }
        }
    }
}
