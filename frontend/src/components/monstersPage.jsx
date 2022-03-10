import React, { Component } from "react";
import Monsters from "./monsters";
import PlayersList from "./playerslist";
import Console from "./console";

class MonstersPage extends Component {
  state = {
    consoleMessage: "",
  };

  render() {
    return (
      <React.Fragment>
        <div className="row">
          <div className="col-sm">
            <h1>Players</h1>
          </div>
          <div className="col-sm">
            <h1>Monsters</h1>
          </div>
          <div className="col-sm">
            <h1>Console</h1>
          </div>
        </div>
        <div className="row">
          <div className="col-sm">
            <PlayersList
              players={this.props.players}
              onSelect={this.props.handlePlayerSelect}
              onChanged={this.props.handleCheckedAllPlayers}
              selectAll={this.props.selectAllPlayers}
            />
          </div>
          <div className="col-sm">
            <Monsters
              onLogin={this.handleLogin}
              onLogout={this.handleLogout}
              onCollectResource={this.handleCollectResource}
              onArchive={this.handleArchive}
              onCrystalPrayer={this.handlePrayer}
              onDemiPower={this.handleDemiPower}
            />
          </div>

          <div className="col-sm">
            <Console
              output={this.state.consoleMessage}
              onClear={this.handleClearConsole}
            />
          </div>
        </div>
      </React.Fragment>
    );
  }
}

export default MonstersPage;
