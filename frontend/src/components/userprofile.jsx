import React, { Component } from "react";

const Userprofile = () => {
  return (
    <React.Fragment>
      <div className="input-group mb-3">
        <div className="input-group-prepend">
          <label className="input-group-text" for="inputGroupSelect01">
            Options
          </label>
        </div>
        <select className="custom-select" id="inputGroupSelect01">
          <option selected>Choose...</option>
          <option value="1">One</option>
          <option value="2">Two</option>
          <option value="3">Three</option>
        </select>
      </div>

      <div className="input-group mb-3">
        <select className="custom-select" id="inputGroupSelect02">
          <option selected>Choose...</option>
          <option value="1">One</option>
          <option value="2">Two</option>
          <option value="3">Three</option>
        </select>
        <div className="input-group-append">
          <label className="input-group-text" for="inputGroupSelect02">
            Options
          </label>
        </div>
      </div>

      <div className="input-group mb-3">
        <div className="input-group-prepend">
          <button className="btn btn-outline-secondary" type="button">
            Button
          </button>
        </div>
        <select
          className="custom-select"
          id="inputGroupSelect03"
          aria-label="Example select with button addon"
        >
          <option selected>Choose...</option>
          <option value="1">One</option>
          <option value="2">Two</option>
          <option value="3">Three</option>
        </select>
      </div>

      <div className="input-group">
        <select
          className="custom-select"
          id="inputGroupSelect04"
          aria-label="Example select with button addon"
        >
          <option selected>Choose...</option>
          <option value="1">One</option>
          <option value="2">Two</option>
          <option value="3">Three</option>
        </select>
        <div className="input-group-append">
          <button className="btn btn-outline-secondary" type="button">
            Button
          </button>
        </div>
      </div>
    </React.Fragment>
  );
};

export default Userprofile;
