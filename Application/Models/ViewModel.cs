using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public class ViewModel
{
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
    public ViewModel(bool status, string message)
    {
        Status = status;
        Message = message;
    }
}

public class ViewModel<T> : ViewModel
{
    public int? Count { get; set; }
    public T? Data { get; set; }
    public List<IdentityError>? Errors { get; set; }
    public ViewModel(bool status, string message):base(status, message) {}
    public ViewModel(bool status, string message, T data) : base(status, message)
    {
        Data = data;
    }
    public ViewModel(bool status, string message, T data, int count) : base(status, message)
    {
        Data = data;
        Count = count;
    }
}