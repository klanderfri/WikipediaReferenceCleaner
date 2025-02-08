namespace ReferenceProcessing
{
    public class Messenger
    {
        public event EventHandler? MessageRaised;

        public void SendErrorMessage(string message) =>
            SendMessage(message, true);

        public void SendMessage(string message, bool IsErrorMessage = false)
        {
            var args = new MessageArgs
            {
                Message = message,
                IsErrorMessage = IsErrorMessage
            };
            SendMessage(args);
        }

        public void SendMessage(MessageArgs e)
        {
            MessageRaised?.Invoke(this, e);
        }
    }
}
