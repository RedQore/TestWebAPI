using TestWebAPI.Models;

namespace TestWebAPI.Services
{
    public class MessageQueue
    {
        private List<ServerMessage> _messages = new();

        public IEnumerable<ServerMessage> GetAll() => _messages;

        public IEnumerable<ClientMessage> GetById(int resipientId)
        {
            return _messages.FindAll(message => message.Recipients.Contains(resipientId)).
                SelectMany(message => new List<ClientMessage>() { new(message.Object, message.Body) });
        }

        public void Put(IEnumerable<ServerMessage> message) => _messages.AddRange(message);

        public IEnumerable<ClientMessage> Pop(int resipientId)
        {
            var clientMessages = new List<ClientMessage>();

            _messages.FindAll(message => message.Recipients.Contains(resipientId)).ForEach(message =>
            {
                clientMessages.Add(new ClientMessage(message.Object, message.Body));
                message.Recipients.Remove(resipientId);
            });

            _messages = _messages.FindAll(message => message.Recipients.Any());

            return clientMessages;
        }
    }
}
