import logo from "./logo.svg";
import "./styles/App.css";
import React, { useState, useEffect } from "react";

// SignalR
import { HubConnectionBuilder } from "@microsoft/signalr";
// Axios
import axios from "axios";
// Redux
import { useSelector, useDispatch } from "react-redux";

// Custom game components
import GameBoard from "./components/GameBoard";
import TeamDisplay from "./components/TeamDisplay";

function App() {
  const gameState = useSelector((state) => state.gameState);
  const [messages, setMessages] = useState([]);
  const [connection, setConnection] = useState(null);

  // ASP.NET API calls

  const createRoom = async () => {
    try {
      const response = await axios.post("/api/game/createRoom");
      // Handle response data (e.g., extract roomId)
    } catch (error) {
      // Handle errors
    }
  };

  const joinRoom = async (roomId, playerName) => {
    try {
      const response = await axios.post(`/api/game/joinRoom/${roomId}`, {
        playerName,
      });
      // Handle response data (e.g., extract teamId)
    } catch (error) {
      // Handle errors
    }
  };

  const changeTeam = async (roomId, newTeamId) => {
    try {
      await axios.post(`/api/game/changeTeam/${roomId}`, { newTeamId });
      // Handle response or display success message
    } catch (error) {
      // Handle errors
    }
  };

  const changeName = async (roomId, newName) => {
    try {
      await axios.post(`/api/game/changeName/${roomId}`, { newName });
      // Handle response or display success message
    } catch (error) {
      // Handle errors
    }
  };

  // SignalR handlers

  const handleGuessSubmit = (guess) => {
    // Send the guess to the backend using SignalR
    // ...
    // Update the game state based on the response from the backend
    // ...
  };



  // SignalR Hub connection

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5281/gamehub")
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
      <GameBoard
        gameState={gameState}
        onGuessSubmit={handleGuessSubmit}
        createRoom={createRoom}
        joinRoom={joinRoom}
      />
      {/* Render TeamDisplay components for each team */}
    </div>
  );

}

export default App;
