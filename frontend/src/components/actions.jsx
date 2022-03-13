import React, { Component } from "react";
import Checkbox from "./checkbox";
//http://react.tips/checkboxes-in-react/

const ambrosia = "1";
const malekus = "2";
const corvintheus = "3";
const aurora = "4";
const azeron = "5";

class Actions extends Component {
  state = {
    colosseumBattleState: false,
  };
  booster = React.createRef();
  customUri = React.createRef();

  render() {
    // console.log("rendering Actions");
    return (
      <div className="actionboxstyle">
        <button
          type="button"
          className="btn btn-success"
          onClick={this.props.onLogin}
        >
          Login
        </button>
        <span> </span>
        <button
          type="button"
          className="btn btn-secondary"
          onClick={this.props.onLogout}
        >
          Logout
        </button>
        <Checkbox
          label={"Colosseum Battle"}
          id={"battleswitch"}
          isSelected={this.props.isSelected}
          onCheckboxChange={this.props.toggleColosseumBattle}
        ></Checkbox>
        <br />
        <br />
        <button
          type="button"
          className="btn btn-primary"
          onClick={this.props.onCollectResource}
        >
          Collect Resource
        </button>
        <br />
        <br />
        <button
          type="button"
          className="btn btn-primary"
          onClick={this.props.onCrystalPrayer}
        >
          Pray (crystal)
        </button>
        <br />
        <br />
        <button
          type="button"
          className="btn btn-primary"
          onClick={this.props.onArchive}
        >
          Archive
        </button>
        <br />
        <br />
        <div>
          <button
            type="button"
            className="btn btn-primary"
            onClick={this.props.onDailySpin}
          >
            Daily Spin
          </button>
        </div>
        <br />
        <div>
          <button
            type="button"
            className="btn btn-primary"
            onClick={this.props.onCollectTerritory}
          >
            Collect Territory
          </button>
        </div>
        <br />
        <button
          type="button"
          className="btn btn-info"
          onClick={() => this.props.onDemiPower(ambrosia)}
        >
          Ambrosia
        </button>
        <button
          type="button"
          className="btn btn-secondary"
          onClick={() => this.props.onDemiPower(azeron)}
        >
          Azeron
        </button>
        <button
          type="button"
          className="btn btn-success"
          onClick={() => {
            this.props.onDemiPower(aurora);
          }}
        >
          Aurora
        </button>
        <button
          type="button"
          className="btn btn-danger"
          onClick={() => this.props.onDemiPower(malekus)}
        >
          Malekus
        </button>
        <button
          type="button"
          className="btn btn-warning"
          onClick={() => this.props.onDemiPower(corvintheus)}
        >
          Corventheus
        </button>
        <div style={{ margin: "20px 0 0 0" }}>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              placeholder="pl. /castle_ws/index.php?"
              ref={this.customUri}
            />
            <small className="form-text text-muted">
              Base URL: https://web3.castleagegame.com
            </small>
          </div>
          <div className="form-group form-check">
            <input
              type="checkbox"
              name="booster"
              ref={this.booster}
              className="form-check-input"
            />
            <label htmlFor="booster" className="form-check-label">
              Booster
            </label>
          </div>
          <div className="form-group">
            <button
              type="button"
              className="btn btn-outline-dark"
              onClick={() =>
                this.props.onCustomUri(
                  this.customUri.current.value,
                  this.booster.current.checked
                )
              }
            >
              Küldés
            </button>
          </div>
        </div>
      </div>
    );
  }
}

export default Actions;
