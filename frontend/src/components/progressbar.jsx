import React, { Component } from "react";

const ProgressBar = ({ now }) => {
  return (
    <div className="progress" style={{ height: "30px" }}>
      <div
        className="progress-bar bg-success"
        role="progressbar"
        aria-valuenow={now}
        aria-valuemin="0"
        aria-valuemax="100"
        label={`${now}%`}
        style={{ width: `${now}%`, borderRadius: "0.5rem" }}
      >
        {now}
      </div>
    </div>
  );
};

export default ProgressBar;
