namespace ExceptionAll.Abstractions.Interfaces;

public interface IDetailBuilder
{
    /// <summary>
    /// Returns the status code and title associated with the API response
    /// </summary>
    /// <returns></returns>
    (int StatusCode, string Title) GetDetails();
}