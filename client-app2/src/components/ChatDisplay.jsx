function ChatDisplay({ messages }) {
  return (
    <div className="chat-display">
      <ul>
        {messages.map((message, index) => (
          <li key={index}>
            <strong>{message.sender}:</strong> {message.text}
          </li>
        ))}
      </ul>
    </div>
  );
}
export default ChatDisplay;
