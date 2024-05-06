const initialState = {
  gameState: 'lobby',
  // ... other initial state properties ...
};

const gameReducer = (state = initialState, action) => {
  switch (action.type) {
    case 'START_GAME':
      return { ...state, gameState: 'inProgress' };
    case 'SUBMIT_CLUE':
      return { ...state, currentClue: action.payload };
    // ... other cases for handling actions ...
    default:
      return state;
  }
};

export default gameReducer;
