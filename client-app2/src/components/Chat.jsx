import React, { useState, useEffect } from 'react';
import ChatDisplay from './ChatDisplay';
import ChatInput from './ChatInput';
import "../styles/Chat.css";

function Chat({ messages, onMessageSend }) {
  const [newMessage, setNewMessage] = useState('');

  // ... handle message sending and other logic ...

  return (
    <div className="chat">
      <ChatDisplay messages={messages} />
      <ChatInput onMessageSend={onMessageSend} />
    </div>
  );
}

export default Chat;
