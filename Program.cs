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
    public static void Main()
    {
        var client = new RestClient("https://api.twitter.com/");
        var settings = JsonConvert.DeserializeObject<OAuthSettings>(File.ReadAllText("settings.json"));

        if (settings?.ConsumerToken == null)
            throw new InvalidOperationException("An invalid consumer token was specified.");
        if (settings?.ConsumerSecret == null)
            throw new InvalidOperationException("An invalid consumer secret was specified.");

        string bearer;

        var authRequest = new RestRequest("oauth2/token", Method.POST);
        client.Authenticator = new HttpBasicAuthenticator(settings.ConsumerToken,settings.ConsumerSecret);
        authRequest.AddParameter("grant_type", "client_credentials");
        var authResponse = client.Execute(authRequest);

        if (authResponse.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException("A token was not obtained successfully. Response: \{authResponse.Content}");

        var token = JsonConvert.DeserializeObject<OAuthToken>(authResponse.Content);

        client.Authenticator = null;

        var searchRequest = new RestRequest("1.1/search/tweets.json");
        searchRequest.AddParameter("q", "asp.net vNext");
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
        public SearchMetadata search_metadata { get; set; }

        public class Metadata
        {
            public string iso_language_code { get; set; }
            public string result_type { get; set; }
        }

        public class Url2
        {
            public string url { get; set; }
            public string expanded_url { get; set; }
            public string display_url { get; set; }
            public List<int> indices { get; set; }
        }

        public class Url
        {
            public List<Url2> urls { get; set; }
        }

        public class Description
        {
            public List<object> urls { get; set; }
        }

        public class Entities
        {
            public Url url { get; set; }
            public Description description { get; set; }
        }

        public class User
        {
            public long id { get; set; }
            public string id_str { get; set; }
            public string name { get; set; }
            public string screen_name { get; set; }
            public string location { get; set; }
            public object profile_location { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public Entities entities { get; set; }
            public bool @protected { get; set; }
            public int followers_count { get; set; }
            public int friends_count { get; set; }
            public int listed_count { get; set; }
            public string created_at { get; set; }
            public int favourites_count { get; set; }
            public object utc_offset { get; set; }
            public object time_zone { get; set; }
            public bool geo_enabled { get; set; }
            public bool verified { get; set; }
            public int statuses_count { get; set; }
            public string lang { get; set; }
            public bool contributors_enabled { get; set; }
            public bool is_translator { get; set; }
            public bool is_translation_enabled { get; set; }
            public string profile_background_color { get; set; }
            public string profile_background_image_url { get; set; }
            public string profile_background_image_url_https { get; set; }
            public bool profile_background_tile { get; set; }
            public string profile_image_url { get; set; }
            public string profile_image_url_https { get; set; }
            public string profile_link_color { get; set; }
            public string profile_sidebar_border_color { get; set; }
            public string profile_sidebar_fill_color { get; set; }
            public string profile_text_color { get; set; }
            public bool profile_use_background_image { get; set; }
            public bool default_profile { get; set; }
            public bool default_profile_image { get; set; }
            public bool following { get; set; }
            public bool follow_request_sent { get; set; }
            public bool notifications { get; set; }
        }

        public class Metadata2
        {
            public string iso_language_code { get; set; }
            public string result_type { get; set; }
        }

        public class Url4
        {
            public string url { get; set; }
            public string expanded_url { get; set; }
            public string display_url { get; set; }
            public List<int> indices { get; set; }
        }

        public class Url3
        {
            public List<Url4> urls { get; set; }
        }

        public class Description2
        {
            public List<object> urls { get; set; }
        }

        public class Entities2
        {
            public Url3 url { get; set; }
            public Description2 description { get; set; }
        }

        public class User2
        {
            public long id { get; set; }
            public string id_str { get; set; }
            public string name { get; set; }
            public string screen_name { get; set; }
            public string location { get; set; }
            public object profile_location { get; set; }
            public string description { get; set; }
            public string url { get; set; }
            public Entities2 entities { get; set; }
            public bool @protected { get; set; }
            public int followers_count { get; set; }
            public int friends_count { get; set; }
            public int listed_count { get; set; }
            public string created_at { get; set; }
            public int favourites_count { get; set; }
            public object utc_offset { get; set; }
            public object time_zone { get; set; }
            public bool geo_enabled { get; set; }
            public bool verified { get; set; }
            public int statuses_count { get; set; }
            public string lang { get; set; }
            public bool contributors_enabled { get; set; }
            public bool is_translator { get; set; }
            public bool is_translation_enabled { get; set; }
            public string profile_background_color { get; set; }
            public string profile_background_image_url { get; set; }
            public string profile_background_image_url_https { get; set; }
            public bool profile_background_tile { get; set; }
            public string profile_image_url { get; set; }
            public string profile_image_url_https { get; set; }
            public string profile_banner_url { get; set; }
            public string profile_link_color { get; set; }
            public string profile_sidebar_border_color { get; set; }
            public string profile_sidebar_fill_color { get; set; }
            public string profile_text_color { get; set; }
            public bool profile_use_background_image { get; set; }
            public bool default_profile { get; set; }
            public bool default_profile_image { get; set; }
            public bool following { get; set; }
            public bool follow_request_sent { get; set; }
            public bool notifications { get; set; }
        }

        public class Url5
        {
            public string url { get; set; }
            public string expanded_url { get; set; }
            public string display_url { get; set; }
            public List<int> indices { get; set; }
        }

        public class Entities3
        {
            public List<object> hashtags { get; set; }
            public List<object> symbols { get; set; }
            public List<object> user_mentions { get; set; }
            public List<Url5> urls { get; set; }
        }

        public class RetweetedStatus
        {
            public Metadata2 metadata { get; set; }
            public string created_at { get; set; }
            public long id { get; set; }
            public string id_str { get; set; }
            public string text { get; set; }
            public bool truncated { get; set; }
            public object in_reply_to_status_id { get; set; }
            public object in_reply_to_status_id_str { get; set; }
            public object in_reply_to_user_id { get; set; }
            public object in_reply_to_user_id_str { get; set; }
            public object in_reply_to_screen_name { get; set; }
            public User2 user { get; set; }
            public object geo { get; set; }
            public object coordinates { get; set; }
            public object place { get; set; }
            public object contributors { get; set; }
            public int retweet_count { get; set; }
            public int favorite_count { get; set; }
            public Entities3 entities { get; set; }
            public bool favorited { get; set; }
            public bool retweeted { get; set; }
            public bool possibly_sensitive { get; set; }
            public string lang { get; set; }
        }

        public class UserMention
        {
            public string screen_name { get; set; }
            public string name { get; set; }
            public long id { get; set; }
            public string id_str { get; set; }
            public List<int> indices { get; set; }
        }

        public class Url6
        {
            public string url { get; set; }
            public string expanded_url { get; set; }
            public string display_url { get; set; }
            public List<int> indices { get; set; }
        }

        public class Entities4
        {
            public List<object> hashtags { get; set; }
            public List<object> symbols { get; set; }
            public List<UserMention> user_mentions { get; set; }
            public List<Url6> urls { get; set; }
        }

        public class Status
        {
            public Metadata metadata { get; set; }
            public string created_at { get; set; }
            public long id { get; set; }
            public string id_str { get; set; }
            public string text { get; set; }
            public string source { get; set; }
            public bool truncated { get; set; }
            public object in_reply_to_status_id { get; set; }
            public object in_reply_to_status_id_str { get; set; }
            public object in_reply_to_user_id { get; set; }
            public object in_reply_to_user_id_str { get; set; }
            public object in_reply_to_screen_name { get; set; }
            public User user { get; set; }
            public object geo { get; set; }
            public object coordinates { get; set; }
            public object place { get; set; }
            public object contributors { get; set; }
            public RetweetedStatus retweeted_status { get; set; }
            public int retweet_count { get; set; }
            public int favorite_count { get; set; }
            public Entities4 entities { get; set; }
            public bool favorited { get; set; }
            public bool retweeted { get; set; }
            public bool possibly_sensitive { get; set; }
            public string lang { get; set; }
        }

        public class SearchMetadata
        {
            public double completed_in { get; set; }
            public long max_id { get; set; }
            public string max_id_str { get; set; }
            public string next_results { get; set; }
            public string query { get; set; }
            public string refresh_url { get; set; }
            public int count { get; set; }
            public int since_id { get; set; }
            public string since_id_str { get; set; }
        }
    }
}
