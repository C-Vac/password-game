import React from 'react';

function TeamDisplay({ team }) {
  return (
    <div className="team">
      <h3>Team {team.teamId}</h3>
      {/* Display player names and indicate clue giver here */}
    </div>
  );
}

export default TeamDisplay;
