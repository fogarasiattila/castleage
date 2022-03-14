import { Component, OnInit } from '@angular/core';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit {
  groups: Group[] = [];
  players: Player[] = [];
  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.playerService.getGroups().subscribe({
      next: (response) => (this.groups = response),
      error: (error) => console.log(error.message),
    });
    this.playerService.getPlayers().subscribe({
      next: (response) => (this.players = response),
      error: (error) => console.log(error.messag),
    });
  }
}
