namespace SimpleMessenger.Core;

public struct SyncOperation<T>
{
    public SyncOperationType Operation { get; set; }
    public T[] Values { get; set; }

    public override string ToString() => $"Operation: {Operation}; Count: {Values?.Length ?? 0}";
}

public enum SyncOperationType : byte
{
    Add,
    Remove,
    Update
}