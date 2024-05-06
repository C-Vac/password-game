import React, { useState, useEffect } from "react";
import "../styles/GameBoard.css";
import axios from "axios";

// Import child components
import TeamDisplay from "./TeamDisplay";
import Chat from "./Chat";

function GameBoard({
  gameState,
  onGuessSubmit,
  createRoom,
  joinRoom,
  changeTeam,
  changeName,
}) {
  const [guess, setGuess] = useState("");
  const [roomId, setRoomId] = useState(null);
  const [teams, setTeams] = useState([]);
  const [currentRound, setCurrentRound] = useState(1); // State to track the current round
  const [currentClue, setCurrentClue] = useState(null); // State to store the current clue
  const [timeRemaining, setTimeRemaining] = useState(5); // State for the timer (initial 5 seconds)
  const [activeTeam, setActiveTeam] = useState(null); // State to track the active team
  const [messages, setMessages] = useState([]); // Define the messages variable

  // Function to handle guess submission
  const handleGuessSubmit = async (guess) => {
    try {
      const response = await axios.post(`/api/game/submitGuess/${roomId}`, {
        guess,
      });
      // ... handle response (e.g., check if guess is correct, update score) ...
    } catch (error) {
      // Handle errors
      console.error(error);
    }
  };

  const onMessageSend = (message) => {
    // Define the onMessageSend function
    axios.post("/api/chat/sendMessage", { message }).then((response) => {
      // Handle the response data (e.g., update the chat messages)
      setMessages((prevMessages) => [...prevMessages, response.data.message]);
    });
  };

  // Function to handle room creation
  const handleCreateRoomClick = async () => {
    try {
      const response = await axios.post("/api/game/createRoom");
      setRoomId(response.data.roomId); // Update roomId state with the response
    } catch (error) {
      // Handle errors
      console.error(error);
    }
  };

  // Function to handle joining a room
  const handleJoinRoomClick = async (roomId, playerName) => {
    try {
      const response = await axios.post(`/api/game/joinRoom/${roomId}`, {
        playerName,
      });
      // ... handle response data (e.g., update team state) ...
    } catch (error) {
      // Handle errors
      console.error(error);
    }
  };

  // Function to handle changing teams
  const handleChangeTeamClick = async (newTeamId) => {
    try {
      await axios.post(`/api/game/changeTeam/${roomId}`, { newTeamId });
      // ... handle response or display success message ...
    } catch (error) {
      // Handle errors
      console.error(error);
    }
  };

  // Function to handle changing player name
  const handleNameChange = async (newName) => {
    try {
      await axios.post(`/api/game/changeName/${roomId}`, { newName });
      // ... handle response or display success message ...
    } catch (error) {
      // Handle errors
      console.error(error);
    }
  };

  // Function to fetch game state
  const fetchGameState = async () => {
    try {
      const response = await axios.get(`/api/game/gameState/${roomId}`);
      setTeams(response.data.teams);
      setCurrentRound(response.data.currentRound);
      setCurrentClue(response.data.currentClue);
      // ... update other game state variables as needed ...
    } catch (error) {
      console.error(error);
    }
  };

  // UseEffect to fetch game state when roomId changes
  useEffect(() => {
    if (roomId) {
      fetchGameState();
    }
  }, [roomId]);

  // useEffect to handle timer logic
  useEffect(() => {
    let intervalId;

    if (timeRemaining > 0) {
      intervalId = setInterval(() => {
        setTimeRemaining((prevTime) => prevTime - 1);
      }, 1000);
    }
    // Clear interval when time runs out or component unmounts
    return () => clearInterval(intervalId);
  }, [timeRemaining]);

  return (
    <div className="game-board">
      {/* Display game information based on gameState */}

      <h2>Round {currentRound}</h2>
      {/* Conditionally display clue if available */}
      {currentClue && <p>Clue: {currentClue}</p>}

      {/* Display teams using TeamDisplay component */}
      {teams.map((team) => (
        <TeamDisplay
          key={team.teamId}
          team={team}
          isActive={team.teamId === activeTeam}
        /> // Pass isActive prop
      ))}
      
      <input
        type="text"
        value={guess}
        onChange={(e) => setGuess(e.target.value)}
      />
      <button onClick={() => onGuessSubmit(guess)}>Submit Guess</button>
      {/* Display chat interface using Chat component */}
      <Chat messages={messages} onMessageSend={onMessageSend} />

      {/* ... other game board elements ... */}
    </div>
  );
}

export default GameBoard;
