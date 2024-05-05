import logo from "./logo.svg";
import "./App.css";
import React, { useState, useEffect } from "react";
import { HubConnectionBuilder } from "@microsoft/signalr";

import GameBoard from "./components/GameBoard";
import Chat from "./components/Chat";
import TeamDisplay from "./components/TeamDisplay";

function App() {
  const [gameState, setGameState] = useState("lobby");
  const [messages, setMessages] = useState([]);

  const handleGuessSubmit = (guess) => {
    // Send the guess to the backend using SignalR
    // ...
    // Update the game state based on the response from the backend
    // ...
  };

  const handleMessageSend = (messageText) => {
    // Send the message to the backend using SignalR
    // ...

    // Update the messages array with the new message
    setMessages([
      ...messages,
      { sender: "currentPlayerName", text: messageText },
    ]);
  };
  const [connection, setConnection] = useState(null);

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/gamehub") // Replace with your backend URL
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => console.log("Connected to SignalR hub"))
        .catch((err) => console.log("Error connecting to SignalR hub:", err));

      connection.on("NewRound", (round) => {
        // Update game state with new round information
        // ...
      });

      // ... other event handlers ...

      connection.on("GuessIncorrect", () => {
        // Display feedback to the player indicating an incorrect guess
        // ...
      });
    }
  }, [connection]);

  return (
    <div className="app">
      <GameBoard gameState={gameState} onGuessSubmit={handleGuessSubmit} />
      <Chat messages={messages} onMessageSend={handleMessageSend} />
      {/* Render TeamDisplay components for each team */}
    </div>
  );
}

export default App;
