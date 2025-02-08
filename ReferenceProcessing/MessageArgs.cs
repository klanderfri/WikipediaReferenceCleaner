namespace ReferenceProcessing
{
    public class MessageArgs : EventArgs
    {
        public required string Message { get; set; }
        public bool IsErrorMessage { get; set; }
    }
}
