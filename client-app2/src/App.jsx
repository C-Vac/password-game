import logo from "./logo.svg";
import "./styles/App.css";
import React, { useState, useEffect } from "react";

// SignalR
import { HubConnectionBuilder } from "@microsoft/signalr";
// Redux
import { useSelector, useDispatch } from "react-redux";
// Custom game components
import GameBoard from "./components/GameBoard";
import Chat from "./components/Chat";
import TeamDisplay from "./components/TeamDisplay";

function App() {
  const gameState = useSelector((state) => state.gameState);
  const [messages, setMessages] = useState([]);
  const [connection, setConnection] = useState(null);

  const handleGuessSubmit = (guess) => {
    // Send the guess to the backend using SignalR
    // ...
    // Update the game state based on the response from the backend
    // ...
  };

  const handleMessageSend = (messageText) => {
    if (connection) {
      connection
        .invoke("SendMessage", { text: messageText })
        .catch((err) => console.log("Error sending message:", err));
    }
    setMessages([
      ...messages,
      { sender: "currentPlayerName", text: messageText },
    ]);
  };

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5000/gamehub") // TODO: Change this to the URL of the SignalR hub and also make sure ASP.NET Core app is running
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
      connection.on("ReceiveMessage", (message) => {
        setMessages([...messages, message]);
      });
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
  }, [connection, messages]);

  return (
    <div className="app">
      <GameBoard gameState={gameState} onGuessSubmit={handleGuessSubmit} />
      <Chat messages={messages} onMessageSend={handleMessageSend} />
      {/* Render TeamDisplay components for each team */}
    </div>
  );
}

export default App;
