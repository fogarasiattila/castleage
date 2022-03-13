import React, { Component } from "react";
import { Route, Switch } from "react-router-dom";
import "./App.css";
import NavBar from "./components/navbar";
import ActionPage from "./components/actionPage.jsx";
import UsersPage from "./components/usersPage.jsx";
import axios from "axios";

class App extends Component {
  state = {
    players: [],
    selectAllPlayersCheckbox: false,
    colosseumBattleState: false,
  };

  updatePlayersAsync = async () => {
    const { data } = await axios.get("http://localhost/api/player/getplayers");
    const tempState = { ...this.state };
    tempState.players.forEach((t) => {
      if (t.isSelected) {
        let user = data.find((d) => t.username === d.username);
        if (user !== undefined) user.isSelected = true;
      }
    });
    tempState.players = data;
    this.setState(tempState);
  };

  handlePlayerSelect = (t) => {
    const stateCopy = { ...this.state };
    const newState =
      stateCopy.players[stateCopy.players.findIndex((f) => f.id === t)];
    newState.isSelected = !newState.isSelected;
    this.setState(newState);
  };

  handleCheckedAllPlayers = () => {
    let tempState = { ...this.state };
    tempState.selectAllPlayersCheckbox = !tempState.selectAllPlayersCheckbox;
    tempState = this.selectAllPlayers(tempState);
    this.setState(tempState);
  };

  selectAllPlayers(state) {
    state.players.forEach((element) => {
      element.isSelected = state.selectAllPlayersCheckbox;
    });
    return state;
  }

  expiredCookie = (p) => {
    const playersCopy = [...this.state.players];
    const index = playersCopy.findIndex((f) => f.username === p);
    playersCopy[index].hasCookie = false;
    this.setState({ players: playersCopy });
  };

  render() {
    return (
      <React.Fragment>
        <NavBar />
        <main className="container-fluid">
          <Switch>
            <Route
              path="/users"
              render={(props) => (
                <UsersPage
                  {...props}
                  players={this.state.players}
                  updatePlayersAsync={this.updatePlayersAsync}
                  handlePlayerSelect={this.handlePlayerSelect}
                />
              )}
            />
            <Route
              path="/"
              render={(props) => {
                return (
                  <ActionPage
                    {...props}
                    players={this.state.players}
                    updatePlayersAsync={this.updatePlayersAsync}
                    handlePlayerSelect={this.handlePlayerSelect}
                    handleCheckedAllPlayers={this.handleCheckedAllPlayers}
                    selectAllPlayers={this.selectAllPlayers}
                    expiredCookie={this.expiredCookie}
                    colosseumBattleStartStopState={
                      this.state.colosseumBattleState
                    }
                    getColosseumBattleStartStopState={
                      this.getColosseumBattleStartStopState
                    }
                  />
                );
              }}
            />
          </Switch>
        </main>
      </React.Fragment>
    );
  }
}

export default App;
