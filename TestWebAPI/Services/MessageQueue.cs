using TestWebAPI.Models;

namespace TestWebAPI.Services
{
    public class MessageQueue
    {
        private Queue<AdminMessage> _messages = new();

        public IEnumerable<AdminMessage> GetAll() => _messages;

        public void Put(IEnumerable<AdminMessage> messages)
        {
            foreach (var message in messages)
                _messages.Enqueue(message);
        }

        public IEnumerable<ClientMessage> Pop(int resipientId)
        {
            var clientMessages = new Queue<ClientMessage>();
            var newAdminMessages = new Queue<AdminMessage>();

            if (!_messages.Any(message => message.Recipients.Contains(resipientId)))
                return clientMessages;

            foreach (var message in _messages)
            {
                if (message.Recipients.Contains(resipientId))
                {
                    clientMessages.Enqueue(new ClientMessage(message.Object, message.Body));

                    message.Recipients.Remove(resipientId);

                    if (!message.Recipients.Any())
                        continue;
                }
                newAdminMessages.Enqueue(message);
            }

            _messages = newAdminMessages;

            return clientMessages;
        }
    }
}
