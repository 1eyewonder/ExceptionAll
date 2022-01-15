namespace ExceptionAll.Models;

/// <summary>
/// Simple record for communicating errors
/// </summary>
/// <param name="Title"></param>
/// <param name="Message"></param>
public record ErrorDetail(string Title, string Message);