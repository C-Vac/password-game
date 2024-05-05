import logo from "./logo.svg";
import "./App.css";
import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

import GameBoard from "./GameBoard";
import Chat from "./Chat";
import TeamDisplay from "./TeamDisplay";

function App() {
  useEffect(() => {
    // Establish SignalR connection and handle incoming messages
    // ...
  }, []);
  return (
    <div className="app">
      <GameBoard gameState={gameState} onGuessSubmit={handleGuessSubmit} />
      <Chat messages={messages} onMessageSend={handleMessageSend} />
      {/* Render TeamDisplay components for each team */}
    </div>
  );
}

export default App;
