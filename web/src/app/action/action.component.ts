import { Component, OnInit } from '@angular/core';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';
import { PlayerService } from '../services/player.service';

@Component({
  selector: 'app-action',
  templateUrl: './action.component.html',
  styleUrls: ['./action.component.css'],
})
export class ActionComponent implements OnInit {
  groups: Group[] = [];
  players: Player[] = [];
  selectedGroups: Group[] = [];
  selectedPlayers: Player[] = [];

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.playerService.groupsState$.subscribe({
      next: (groups) => {
        this.groups = groups;
        this.selectedGroups = groups;
      },
    });
    this.playerService.playersState$.subscribe({
      next: (players) => {
        this.players = players;
        this.selectedPlayers = players;
      },
    });
  }

  onGroupSelected() {}
  onPlayerSelected() {}
}
