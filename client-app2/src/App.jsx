import logo from "./logo.svg";
import "./App.css";
import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

import GameBoard from "./components/GameBoard";
import Chat from "./components/Chat";
import TeamDisplay from "./components/TeamDisplay";

function App() {
  useEffect(() => {
    // Establish SignalR connection and handle incoming messages
    // ...
  }, []);
  const [gameState, setGameState] = useState("lobby"); // Example using a string
  const handleGuessSubmit = (guess) => {
    // Send the guess to the backend using SignalR
    // ...
    // Update the game state based on the response from the backend
    // ...
  };
  const [messages, setMessages] = useState([]);
  const handleMessageSend = (messageText) => {
    // Send the message to the backend using SignalR
    // ...

    // Update the messages array with the new message
    setMessages([
      ...messages,
      { sender: "currentPlayerName", text: messageText },
    ]);
  };
  return (
    <div className="app">
      <GameBoard gameState={gameState} onGuessSubmit={handleGuessSubmit} />
      <Chat messages={messages} onMessageSend={handleMessageSend} />
      {/* Render TeamDisplay components for each team */}
    </div>
  );
}

export default App;
