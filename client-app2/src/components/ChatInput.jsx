import React, { useState } from "react";

function ChatInput({ onMessageSend }) {
  const [messageText, setMessageText] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    onMessageSend(messageText);
    setMessageText("");
  };

  return (
    <form className="chat-input" onSubmit={handleSubmit}>
      <input
        type="text"
        value={messageText}
        onChange={(e) => setMessageText(e.target.value)}
      />
      <button type="submit">Send</button>
    </form>
  );
}

export default ChatInput;
