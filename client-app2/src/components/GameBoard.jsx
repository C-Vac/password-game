import React, { useState, useEffect } from 'react';
import "../styles/GameBoard.css"

function GameBoard({ gameState, onGuessSubmit }) {
  const [guess, setGuess] = useState('');

  // ... handle guess submission and other logic ...

  return (
    <div className="game-board">
      {/* Display round, scores, clues, guesses, and timer here */}
      <input type="text" value={guess} onChange={(e) => setGuess(e.target.value)} />
      <button onClick={() => onGuessSubmit(guess)}>Submit Guess</button>
    </div>
  );
}

export default GameBoard;
