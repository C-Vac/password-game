import React, { useState } from "react";

function ChatInput({ onMessageSend }) {
  const [messageText, setMessage] = useState("");

const handleSendMessage = () => {
  if (messageText.trim() !== "") {
    onMessageSend(messageText); // Pass message to parent component
    setMessage(""); // Clear input field
  }
};
  return (
    <form className="chat-input" onSubmit={handleSendMessage}>
      <input
        type="text"
        value={messageText}
        onChange={(e) => setMessage(e.target.value)}
      />
      <button onClick={handleSendMessage}>Send</button>
    </form>
  );
}

export default ChatInput;
