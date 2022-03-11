import React, { Component } from "react";
import Actions from "./actions";
import ProgressBar from "./progressbar";
import Console from "./console";
import PlayersList from "./playerslist";
import axios from "axios";

class ActionPage extends Component {
  state = {
    consoleMessage: "",
  };

  async componentDidMount() {
    try {
      await this.props.updatePlayersAsync();
    } catch (ex) {
      let tempConsoleState = this.state.consoleMessage;
      tempConsoleState += `\n` + ex;
      this.setState({ consoleMessage: tempConsoleState });
    }
  }

  handleClearConsole = () => {
    this.setState({ consoleMessage: "" });
  };

  handleLogout = async () => {
    await this.actionOnPlayers((p) => this.sendLogoutRequestAsync(p));
  };

  handleLogin = async () => {
    await this.actionOnPlayers((p) => this.sendLoginRequestAsync(p));
  };

  handleCollectResource = async () => {
    await this.actionOnPlayers((p) => this.sendCollectResourceRequestAsync(p));
  };

  handleArchive = async () => {
    await this.actionOnPlayers((p) => this.sendArchiveRequestAsync(p));
  };

  handleDailySpin = async () => {
    await this.actionOnPlayers((p) => this.sendDailySpinRequestAsync(p));
  };

  handleCollectTerritory = async () => {
    await this.actionOnPlayers((p) => this.sendCollectTerritoryAsync(p));
  };

  handlePrayer = async () => {
    await this.actionOnPlayers((p) => this.sendCrystalPrayerRequestAsync(p));
  };

  handleDemiPower = async (id) => {
    await this.actionOnPlayersParameter(
      (p) => this.sendDemiPowerRequestAsync(p, id),
      id
    );
  };

  handleCustomUri = async (uri, booster) => {
    await this.actionOnPlayersParameter((p) =>
      this.sendCustomUriRequestAsync(p, uri, booster)
    );
  };

  // handleColosseum = async () => {
  //   await this.actionOnPlayers((p) => this.sendColosseumBattleAsync(p));
  // };

  async actionOnPlayers(action) {
    const selectedPlayers = this.props.players.filter((p) => p.isSelected);
    let tasks = selectedPlayers.map(async (p) => await action(p));
    await this.progress(tasks);
    console.log("ez nem jelenhet meg mielőtt elfogytak a taskok");
    await this.props.updatePlayersAsync();
  }

  async actionOnPlayersParameter(action, id) {
    const selectedPlayers = this.props.players.filter((p) => p.isSelected);
    let tasks = selectedPlayers.map(async (p) => await action(p, id));
    await this.progress(tasks);
    await this.props.updatePlayersAsync();
  }

  async progress(tasks) {
    let progress = 0;
    this.setState({ actionProgress: 0 });
    let increment = 100 / tasks.length;

    await Promise.allSettled(
      tasks.map(async (task) => {
        console.log("task started");
        try {
          const result = await task;

          const player = JSON.parse(result.config.data);

          if (result.status === 202) {
            this.sendLoginRequestAsync(player);
            result.data = "Login failed, retrieving new cookie...";
          }
          this.setState({
            consoleMessage:
              this.state.consoleMessage +
              `(${player.username})` +
              result.data +
              `\n`,
          });
        } catch (e) {}
        progress += increment;
        this.setState({ actionProgress: Math.round(progress) });
        console.log("task finished");
      })
    );
  }

  async sendLoginRequestAsync(player) {
    return await axios.post(`http://localhost/api/session/login`, {
      username: player.username,
    });
  }

  async sendLogoutRequestAsync(player) {
    return await axios.post(`http://localhost/api/session/logout`, {
      username: player.username,
    });
  }

  async sendCollectResourceRequestAsync(player) {
    return await axios.post(`http://localhost/api/action/collectresource`, {
      username: player.username,
    });
  }

  async sendArchiveRequestAsync(player) {
    return await axios.post(`http://localhost/api/action/archive`, {
      username: player.username,
    });
  }

  async sendDailySpinRequestAsync(player) {
    return await axios.post(`http://localhost/api/action/dailyspin`, {
      username: player.username,
    });
  }

  async sendCollectTerritoryAsync(player) {
    return await axios.post(`http://localhost/api/action/collectterritory`, {
      username: player.username,
    });
  }

  async sendCrystalPrayerRequestAsync(player) {
    return await axios.post(`http://localhost/api/action/crystalprayer`, {
      username: player.username,
    });
  }

  async sendDemiPowerRequestAsync(player, id) {
    return await axios.post(`http://localhost/api/action/demipower`, {
      username: player.username,
      demigodId: id,
    });
  }

  async sendCustomUriRequestAsync(player, uri, booster) {
    return await axios.post("http://localhost/api/action/customUri", {
      username: player.username,
      uri,
      booster,
    });
  }

  // async sendColosseumBattleAsync(player) {
  //   return await axios.post("http://localhost/api/action/colosseum", {
  //     username: player.username,
  //   });
  // }

  render() {
    return (
      <React.Fragment>
        <div className="row">
          <div className="col-sm">
            <h1>Players</h1>
          </div>
          <div className="col-sm">
            <h1>Actions</h1>
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
            <Actions
              onLogin={this.handleLogin}
              onLogout={this.handleLogout}
              onCollectResource={this.handleCollectResource}
              onCrystalPrayer={this.handlePrayer}
              onArchive={this.handleArchive}
              onDailySpin={this.handleDailySpin}
              onCollectTerritory={this.handleCollectTerritory}
              onDemiPower={this.handleDemiPower}
              onCustomUri={this.handleCustomUri}
              // onColosseum={this.handleColosseum}
            />
          </div>

          <div className="col-sm">
            <Console
              output={this.state.consoleMessage}
              onClear={this.handleClearConsole}
            />
          </div>
        </div>
        <div className="row">
          <div className="col-2" />

          <div className="col-sm">
            <br />
            <br />
            <ProgressBar now={this.state.actionProgress} />
          </div>
          <div className="col-2" />
        </div>
      </React.Fragment>
    );
  }
}

export default ActionPage;
