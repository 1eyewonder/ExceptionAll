namespace ExceptionAll.Client.Interfaces;

/// <summary>
/// HttpClient wrapper service for cleaner top level code
/// </summary>
public interface IExceptionAllClient
{
    /// <summary>
    /// Executes Http Get and returns the requested object type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativeUrl"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask<T?> GetContentAsync<T>(string relativeUrl, CancellationToken token = default);

    /// <summary>
    /// Executes Http Delete and returns the requested object type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativeUrl"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask<T?> DeleteContentAsync<T>(string relativeUrl, CancellationToken token = default);

    /// <summary>
    /// Executes Http Post and returns the requested object type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativeUrl"></param>
    /// <param name="content"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask<T?> PostContentAsync<T>(string relativeUrl, T content, CancellationToken token = new());

    /// <summary>
    /// Executes Http Put and returns the requested object type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="relativeUrl"></param>
    /// <param name="content"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    ValueTask<T?> PutContentAsync<T>(string relativeUrl, T content, CancellationToken token = new());
}