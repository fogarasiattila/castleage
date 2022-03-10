import React, { Component } from "react";

const UsersList = ({ players, onSelect }) => {
  return (
    <React.Fragment>
      <div className="playersboxstyle">
        <ul>
          {players.map(p => (
            <li key={p.id} onClick={() => onSelect(p.id)}>
              {p.username}
            </li>
          ))}
        </ul>
      </div>
    </React.Fragment>
  );
};

export default UsersList;
