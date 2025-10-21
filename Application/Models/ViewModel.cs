namespace Application.Models;

public class ViewModel
{
    public string Message { get; set; } = string.Empty;
    public bool Status { get; set; } = true;
}

public class ViewModel<T> : ViewModel
{
    public int Count { get; set; } = 0;
    public T? Data { get; set; }
}