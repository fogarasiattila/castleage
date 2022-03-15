import { SelectionModel } from '@angular/cdk/collections';
import { SelectorMatcher } from '@angular/compiler';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatListOption } from '@angular/material/list';
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
  filteredPlayers: Player[] = [];
  selectedPlayers: Player[] = [];

  form: FormGroup = new FormGroup({
    groupFilter: new FormControl([]),
    playerSelection: new FormControl(null),
  });

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.playerService.getGroups().subscribe({
      next: (response) => {
        this.groups = response;
        this.form.patchValue({ groupFilter: this.groups[0] });
      },
      error: (error) => console.log(error.message),
    });
    this.playerService.getPlayers().subscribe({
      next: (response) => {
        this.players = response;
        this.filteredPlayers = response;
      },
      error: (error) => console.log(error.messag),
    });
  }

  onPlayerSelected(options: MatListOption[]) {
    // options.forEach((o) => console.log(o.value));
    // console.log(this.selectedPlayers);
  }

  onGroupSelection(group: Group) {
    this.filteredPlayers = this.players.filter((p) =>
      p.memberOf.includes(group.id)
    );
    this.selectedPlayers = this.filteredPlayers.filter((p) =>
      this.selectedPlayers.includes(p)
    );
  }
}
