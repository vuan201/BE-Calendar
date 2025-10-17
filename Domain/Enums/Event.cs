namespace Domain.Enums.Event;

public enum Priolity
{
    NotImportant,
    Default,
    Important,
    Urgent
}
public enum EventType
{
    Meeting,
    Task,
    Reminder,
    Birthday,
    Holiday,
    BusinessTrip,
    Personal,
    Other,
}
public enum RecurrenceRule
{
    NoRepeat,
    Daily,
    Weekly,
    Monthly,
    Yearly,
    Custom,
}