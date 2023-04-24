using Newtonsoft.Json;
using RestSharp;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Net.Essentials.Wiki
{
    public class WikidataClient
    {
        const string ImagePropertyId = "P18";
        RestClient _client;
        RestClient Client
        {
            get
            {
                if (_client == null) _client = new RestClient();
                return _client;
            }
        }

        public async Task<string> FetchExtractAsync(string url)
        {
            try
            {
                if (url == null) return null;
                url = url
                    .Replace("https://", "")
                    .Replace(".wikipedia.org/wiki", "");
                var slashIndex = url.IndexOf("/");
                var site = url.Substring(0, slashIndex);
                var title = url.Substring(slashIndex + 1, url.Length - slashIndex - 1);
                title = title.Replace("_", " ");
                url = $"https://{site}.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exsentences=10&explaintext=1&exlimit=1&titles={title}";

                var request = new RestRequest(url, Method.Get);
                var response = await Client.ExecuteAsync(request);
                var extracts = JsonConvert.DeserializeObject<WikipediaExtractsResponse>(response.Content);
                var textExtracts = extracts?.query?.pages?.Values?.ToDictionary(x => x.title);
                return textExtracts?.Values?.FirstOrDefault()?.extract ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task<List<Sitelink>> GetSitelinksForWikidataIdAsync(string id)
        {
            return await GetSitelinksForWikidataIdsAsync(new[] { id });
        }

        public async Task<List<Sitelink>> GetSitelinksForWikidataIdsAsync(string[] ids)
        {
            var entities = await GetEntitiesAsync(ids);
            if (entities == null) return null;
            var results = new List<Sitelink>();
            foreach (var entity in entities)
            {
                if (entity.sitelinks == null) continue;
                foreach (var site in entity.sitelinks)
                {
                    var code = site.Key.Replace("wiki", "");
                    var href = site.Value.title.Replace(" ", "_");
                    var url = $"https://{code}.wikipedia.org/wiki/{href}";
                    results.Add(new Sitelink
                    {
                        WikidataId = entity.id,
                        Site = site.Key,
                        Title = site.Value.title,
                        Url = url,
                    });
                }
            }
            return results;
        }

        static string GetWikidataEntitiesUrl(string[] entityIds)
        {
            var ids = string.Join("|", entityIds);
            return $"https://www.wikidata.org/w/api.php?action=wbgetentities&format=json&props=sitelinks&ids={ids}";
        }

        async Task<List<WikidataEntity>> GetEntitiesAsync(string[] entityIds)
        {
            var url = GetWikidataEntitiesUrl(entityIds);
            var request = new RestRequest(url, Method.Get);
            var response = await Client.ExecuteAsync(request);
            var entities = JsonConvert.DeserializeObject<WikidataEntitiesResponse>(response.Content);
            return entities?.entities?.Values.ToList();
        }

        string GetWikidataClaimsUrl(string entityId)
        {
            return $"https://www.wikidata.org/w/api.php?action=wbgetclaims&entity={entityId}&format=json&property={ImagePropertyId}";
        }

        async Task<WikidataClaimsResponse> GetClaimsAsync(string entityId)
        {
            var url = GetWikidataClaimsUrl(entityId);
            var request = new RestRequest(url, Method.Get);
            var response = await Client.ExecuteAsync(request);
            var claims = JsonConvert.DeserializeObject<WikidataClaimsResponse>(response.Content);
            return claims;
        }

        public async Task<List<string>> GetImagesForWikidataAsync(string entityId)
        {
            var claims = await GetClaimsAsync(entityId);
            if (claims == null || claims.claims == null || !claims.claims.ContainsKey(ImagePropertyId)) return null;
            var fileNames = claims.claims[ImagePropertyId].Select(x => x.mainsnak?.datavalue?.value).Where(x => x != null);
            var images = fileNames.Select(x => GetWikidataImageUrl(x));
            return images.ToList();
        }

        string GetWikidataImageUrl(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return null;

            return $"https://commons.wikimedia.org/w/index.php?title=Special:Redirect/file/{filename}";
        }

        class WikipediaExtractsResponse
        {
            public WikipediaExtractsQuery query { get; set; }
        }

        class WikipediaExtractsQuery
        {
            public Dictionary<string, WikipediaExtractsPage> pages { get; set; }
        }

        class WikipediaExtractsPage
        {
            public string pageid { get; set; }
            public string title { get; set; }
            public string extract { get; set; }
        }

        class WikidataEntitiesResponse
        {
            public Dictionary<string, WikidataEntity> entities { get; set; }
        }

        class WikidataEntity
        {
            public string id { get; set; }
            public Dictionary<string, WikidataSitelink> sitelinks { get; set; }
        }

        class WikidataSitelink
        {
            public string site { get; set; }
            public string title { get; set; }
        }

        class WikidataClaimsResponse
        {
            public Dictionary<string, WikidataClaim[]> claims { get; set; }
        }

        class WikidataClaim
        {
            public WikidataSnak mainsnak { get; set; }
        }

        class WikidataSnak
        {
            public WikidataDataValue datavalue { get; set; }
        }

        class WikidataDataValue
        {
            public string value { get; set; }
        }
    }
}