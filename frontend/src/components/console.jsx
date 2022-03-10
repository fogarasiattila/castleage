import React, { Component } from "react";

const Console = ({ output, onClear }) => {
  return (
    <React.Fragment>
      <div className="consoleboxstyle">
        <pre className="console-body">
          <output>{output}</output>
        </pre>
      </div>
      <button onClick={onClear}>Clear</button>
    </React.Fragment>
  );
};

export default Console;
