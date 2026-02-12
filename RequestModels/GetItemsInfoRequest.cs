namespace FeverApi.RequestModels;

public class GetItemsInfoRequest
{
    /// <summary>
    /// По списку id новостей
    /// </summary>
    public IReadOnlyList<string>? Ids { get; init; }
    
    /// <summary>
    /// фильтр источнику новости
    /// </summary>
    public IReadOnlyList<int>? FeedIds { get; init; }
    
    /// <summary>
    /// по группе источников новостей
    /// </summary>
    public IReadOnlyList<int>? GroupIds { get; init; }
    
    /// <summary>
    /// Усекает до указанной новости (не включая ее)
    /// </summary>
    public string? MaxId { get; init; }
 
    /// <summary>
    /// получение новостей после указанной (не включая ее)
    /// </summary>
    public string? SinceId { get; init; }
}