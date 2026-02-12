using FeverApi.RequestModels;

namespace FeverApi.Extensions;

public static class GetItemsInfoRequestExtensions
{
    /// <summary>
    /// признак наличия параметров запроса
    /// </summary>
    public static bool HaveParams(this GetItemsInfoRequest request) =>
        request.Ids is { Count: > 0 } ||
        request.FeedIds is { Count: > 0 } ||
        request.GroupIds is { Count: > 0 } ||
        request.MaxId is not null ||
        request.SinceId is not null;
}