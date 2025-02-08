namespace ReferenceProcessing
{
    public class Messenger
    {
        public event EventHandler? MessageRaised;

        public void SendMessage(string message)
        {
            var args = new MessageArgs
            {
                Message = message
            };
            SendMessage(args);
        }

        public void SendMessage(MessageArgs e)
        {
            MessageRaised?.Invoke(this, e);
        }
    }
}
