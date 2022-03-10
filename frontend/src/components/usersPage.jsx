import React, { Component } from "react";
import PlayersList from "./playerslist";
import Console from "./console";
import axios from "axios";

class UsersPage extends Component {
  newPlayer = {
    displayName: "New player",
    username: "",
    playerCode: "",
    armyCode: "",
    password: "",
  };

  state = {
    consoleMessage: "",
    selectedPlayer: this.newPlayer,
  };

  async componentDidMount() {
    await this.props.updatePlayersAsync();
  }

  async componentDidUpdate(prevProps, prevState) {
    // console.log(prevProps);
    // console.log(prevState);
    // vigyázz, az update miatt végtelen cikklusba kerülhet ez a hívás, ha megváltozik a state, pl. updatePlayerAsync() esetén - használj if() -et
  }

  handleClearConsole = () => {
    this.setState({ consoleMessage: "" });
  };

  onSelectPlayer = (p) => {
    const selectedPlayer = this.props.players.find(
      (ply) => ply.id == p.target.value
    );
    let tempStatePlayer = { ...this.state.selectedPlayer };
    tempStatePlayer = selectedPlayer ? selectedPlayer : this.newPlayer;
    tempStatePlayer.password = "";
    this.setState({ selectedPlayer: tempStatePlayer });
  };

  handleSubmit = async (e) => {
    e.preventDefault();

    let message;

    if (this.state.selectedPlayer.id) {
      await axios.post(
        "http://localhost/api/player/modify",
        this.state.selectedPlayer
      );
      message = `Modified player: ${this.state.selectedPlayer.username}`;
    } else {
      await axios.post(
        "http://localhost/api/player/new",
        this.state.selectedPlayer
      );
      message = `New player added: ${this.state.selectedPlayer.username}`;
    }

    this.setState({
      consoleMessage: this.state.consoleMessage + `${message}` + `\n`,
    });

    await this.props.updatePlayersAsync();
  };

  handleChange = (e) => {
    const tempStatePlayer = { ...this.state.selectedPlayer };
    tempStatePlayer[e.currentTarget.name] = e.currentTarget.value;
    this.setState({ selectedPlayer: tempStatePlayer });
  };

  handleDelete = async () => {
    // const config = { headers: { "Content-Type": "application/json" } };

    if (this.state.selectedPlayer.id > 0)
      await axios.delete(
        `http://localhost/api/player/delete/${this.state.selectedPlayer.id}`
      );

    this.setState({
      consoleMessage:
        this.state.consoleMessage +
        `Deleted user ${this.state.selectedPlayer.username}` +
        `\n`,
      selectedPlayer: this.newPlayer,
    });

    await this.props.updatePlayersAsync();
  };

  handleClearConsole = () => {
    this.setState({ consoleMessage: "" });
  };

  render() {
    const { selectedPlayer: account } = this.state;
    return (
      <React.Fragment>
        <div className="form-group">
          <select
            className="custom-select"
            required
            onChange={this.onSelectPlayer}
          >
            <option value="0">New player</option>
            {this.props.players.map((p) => (
              <option value={p.id} key={p.id}>
                {p.username}
              </option>
            ))}
          </select>
          <div className="invalid-feedback">
            Example invalid custom select feedback
          </div>
        </div>
        <form
          className="needs-validation"
          noValidate
          onSubmit={this.handleSubmit}
        >
          <div className="form-row">
            <div className="col-md-4 mb-3">
              <label htmlFor="validationTooltip02">Full name</label>
              <input
                type="text"
                className="form-control"
                id="validationTooltip02"
                placeholder="name"
                value={account.displayName}
                required
                onChange={this.handleChange}
                name="displayName"
              />
              <div className="valid-tooltip">Looks good!</div>
            </div>
            <div className="col-md-4 mb-3">
              <label htmlFor="validationTooltipUsername">Username</label>
              <div className="input-group">
                <div className="input-group-prepend">
                  <span
                    className="input-group-text"
                    id="validationTooltipUsernamePrepend"
                  >
                    @
                  </span>
                </div>
                <input
                  type="text"
                  className="form-control"
                  id="validationTooltipUsername"
                  placeholder="Username"
                  aria-describedby="validationTooltipUsernamePrepend"
                  value={account.username}
                  onChange={this.handleChange}
                  required
                  name="username"
                />
                <div className="invalid-tooltip">
                  Please choose a unique and valid username.
                </div>
              </div>
            </div>
          </div>
          <div className="form-row">
            <div className="col-md-6 mb-3">
              <label htmlFor="validationTooltip03">Password</label>
              <input
                type="password"
                className="form-control"
                id="validationTooltip03"
                placeholder="password"
                value={account.password}
                required
                onChange={this.handleChange}
                name="password"
              />
              <div className="invalid-tooltip">
                Please provide a valid city.
              </div>
            </div>
            <div className="col-md-3 mb-3">
              <label htmlFor="validationTooltip04">Playercode</label>
              <input
                type="text"
                className="form-control"
                id="validationTooltip04"
                placeholder="playerCode"
                value={account.playerCode ? account.playerCode : ""}
                required
                onChange={this.handleChange}
                name="playerCode"
              />
              <div className="invalid-tooltip">
                Please provide a valid state.
              </div>
            </div>
            <div className="col-md-3 mb-3">
              <label htmlFor="validationTooltip05">ArmyCode</label>
              <input
                type="text"
                className="form-control"
                id="validationTooltip05"
                placeholder="armycode"
                value={account.armyCode ? account.armyCode : ""}
                required
                onChange={this.handleChange}
                name="armyCode"
              />
              <div className="invalid-tooltip">Please provide a valid zip.</div>
            </div>
          </div>
          <div className="btn-group mr-2">
            <button className="btn btn-primary" type="submit">
              Save
            </button>
          </div>
          <div className="btn-group">
            <button
              className="btn btn-primary"
              type="button"
              onClick={this.handleDelete}
            >
              Delete
            </button>
          </div>
        </form>
        <div className="col-sm">
          <Console
            output={this.state.consoleMessage}
            onClear={this.handleClearConsole}
          />
        </div>
      </React.Fragment>
    );
  }
}

export default UsersPage;
