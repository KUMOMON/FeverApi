using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FeverApi.Converters;
using FeverApi.Extensions;
using FeverApi.RequestModels;
using FeverApi.Responses;
using FeverApi.Responses.FeedItems;
using FeverApi.Responses.Feeds;
using RestSharp;

namespace FeverApi;

public class FeverApiClient
{
    protected readonly IRestClient RestClient;
    protected readonly JsonSerializerOptions JsonSerializerOptions;

    /// <param name="url">
    /// address api endpoint
    /// <example>http://some.domain/api/fever.php</example>
    /// </param>
    /// <param name="user">username</param>
    /// <param name="apiPassword">special password for api query</param>
    /// <exception cref="ArgumentNullException">if one of parameters is null</exception>
    public FeverApiClient(string url, string user, string apiPassword)
    {
        if (url is null)
            throw new ArgumentNullException(url);
        if (user is null)
            throw new ArgumentNullException(user);
        if (apiPassword is null)
            throw new ArgumentNullException(apiPassword);
        
        var token = ConvertCredentialToToken(user, apiPassword);
        
        RestClient = new RestClient(url);
        RestClient.AddDefaultParameter("api_key", token);
        JsonSerializerOptions = new JsonSerializerOptions()
        {
            Converters =
            {
                new EpochConverter(),
                new BoolConverter(),
                new CommaSeparatedListStringsConverter(),
                new CommaSeparatedListIntsConverter()
            },
        };
    }

    public async Task<bool> CheckIsAuthentificated(CancellationToken cancellationToken = default)
    {
        var response = await PostRestRequest<GetUnreededFeedItemsIdsResponse>(string.Empty, cancellationToken);
        return response.Auth;
    }

    public async Task<GetGroupsResponse> GetGroups(CancellationToken cancellationToken = default)
    {
        return await PostRestRequest<GetGroupsResponse>("?groups", cancellationToken);
    }
    
    public async Task<GetFeedsResponse<T>> GetFeeds<T>(CancellationToken cancellationToken = default) where T : FeedShort
    {
        return await PostRestRequest<GetFeedsResponse<T>>("?feeds", cancellationToken);
    }
    
    public async Task<GetUnreededFeedItemsIdsResponse> GetUnreadNewsIds(CancellationToken cancellationToken = default)
    {
        return await PostRestRequest<GetUnreededFeedItemsIdsResponse>("?unread_item_ids", cancellationToken);
    }
    
    public async IAsyncEnumerable<T> GetItemsInfo<T>(GetItemsInfoRequest getItemsInfoRequest, [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : ItemShort
    {
        if(getItemsInfoRequest?.HaveParams() is not true)
            yield break;

        //так уж сложилось что если указывать id в запросе, то он всегда отдается, несмотря на `max_id` и `since_id`
        //поэтому в случае их получения будем исключать из запроса чтобы не уйти в бесконечный цикл
        SortedSet<string> searchIds = getItemsInfoRequest.Ids is not null 
            ? new SortedSet<string>(getItemsInfoRequest.Ids) 
            : [];
        
        var param = new Dictionary<string, IEnumerable<string>?>()
        {
            { "with_ids", searchIds },
            { "feed_ids", getItemsInfoRequest.FeedIds?.Select(x=>Convert.ToString(x)) },
            { "group_ids", getItemsInfoRequest.GroupIds?.Select(x=>Convert.ToString(x)) }
        };
        if (string.IsNullOrWhiteSpace(getItemsInfoRequest.MaxId) is false)
        {
            param["max_id"] = [getItemsInfoRequest.MaxId];
        }
        
        string? lastId = default;
        if (getItemsInfoRequest.SinceId is not null)
            lastId = getItemsInfoRequest.SinceId;
        
        do
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (string.IsNullOrWhiteSpace(lastId) is false)
            {
                param["since_id"] = [lastId];
            }
            
            var query = BuildQueryString(param);
            var responseModel = (await PostRestRequest<GetItemsResponse<T>>("?items&" + query, cancellationToken));
            foreach (var item in responseModel.Items ?? [])
            {
                if (searchIds.Contains(item.Id))
                {
                    searchIds.Remove(item.Id);
                    param["with_ids"] = searchIds;
                }
                yield return item;
            }
            lastId = responseModel.Items?.LastOrDefault()?.Id;
        } while (lastId is not null);
    }
    
    
    protected async Task<T> PostRestRequest<T>(string api, CancellationToken cancellationToken = default)
    {
        var request = new RestRequest(api, Method.Post) { AlwaysMultipartFormData = true };
        var response = await RestClient.PostAsync(request, cancellationToken);

        if (response == null)
            throw new Exception("Failed to get response from Fever API");

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Error: {response.StatusCode} - {response.Content}");

        if (response.Content == null)
            throw new Exception("Response content is null");

        try
        {
            var responseObject = JsonSerializer.Deserialize<T>(response.Content, JsonSerializerOptions);
            if (responseObject == null)
                throw new Exception($"Failed to deserialize response content - {response.Content}");

            return responseObject;
        }
        catch (Exception e)
        {
            throw new Exception($"Failed to deserialize response content - {response.Content}", e);
        }
    }
    
    /// <summary>
    /// Билдит параметры запроса для строки запроса без `?`
    /// </summary>
    /// <param name="parameters">ключи [параметр]=[значения]. Если значений несколько, то они будут возвращены через `,`</param>
    protected static string BuildQueryString(Dictionary<string, IEnumerable<string>?> parameters)
    {
        var queryParts = new List<string>(parameters.Count);
        foreach (var param in parameters)
        {
            var values = string.Join(",", param.Value ?? []);
            if(string.IsNullOrWhiteSpace(values) is false)
                queryParts.Add($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(values)}");
        }
        return string.Join("&", queryParts);
    }
    
    
    /// <remarks>echo -n "user:apiPass" | md5sum | cut -d' ' -f1</remarks>
    private static string ConvertCredentialToToken(string user, string apiPassword)
    {
        using var hasher = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes($"{user}:{apiPassword}");
        var hashBytes = hasher.ComputeHash(inputBytes);
        string token = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return token;
    }
}