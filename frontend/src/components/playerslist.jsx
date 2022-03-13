import React from "react";

const PlayersList = ({ players, onSelect, onChanged, selectAll }) => {
  return (
    <React.Fragment>
      <div className="playersboxstyle">
        <ul>
          {players.map((p) => (
            <li
              key={p.id}
              className={p.isSelected ? "playerslist active" : "playerslist"}
              onClick={() => onSelect(p.id)}
            >
              {p.hasCookie ? (
                <span className="rendercookiestyle">C</span>
              ) : (
                <span className="rendernocookiestyle">X</span>
              )}{" "}
              {p.username}
            </li>
          ))}
        </ul>
      </div>
      <input
        type="checkbox"
        onChange={onChanged}
        // handleCheckboxChange={selectAll}
        className="selectall-checkbox"
      />
    </React.Fragment>
  );
};

export default PlayersList;
