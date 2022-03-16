import { SelectionModel } from '@angular/cdk/collections';
import { SelectorMatcher } from '@angular/compiler';
import {
  Component,
  Input,
  OnInit,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatListOption } from '@angular/material/list';
import { Observable } from 'rxjs';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit, OnChanges {
  _groups: Group[] = [];
  _players: Player[] = [];

  @Input()
  set groups(value: Group[]) {
    if (value[0] === null) return;
    this._groups = value;
    this.form.patchValue({ groupFilter: value[0] });
    this.filteredPlayers = this.players;
  }
  get groups() {
    return this._groups;
  }

  @Input()
  set players(value) {
    if (value[0] === null) return;
    this._players = value;
    this.filteredPlayers = value.filter((p) =>
      p.memberOf.has(this.form.get('groupFilter').value.id)
    );
  }
  get players() {
    return this._players;
  }

  filteredPlayers: Player[] = [];
  selectedPlayers: Player[] = [];

  form: FormGroup = new FormGroup({
    groupFilter: new FormControl([]),
  });

  constructor(private playerService: PlayerService) {}

  ngOnChanges(changes: SimpleChanges): void {
    console.log('groups-list.component onChanges');
    console.log(changes);
  }

  ngOnInit(): void {
    console.log('groups-list.component init');
    // this.groups$.subscribe((gs) => (this.groups = gs));
  }

  onPlayerSelected(options: MatListOption[]) {
    // options.forEach((o) => console.log(o.value));
    // console.log(this.selectedPlayers);
  }

  onGroupSelection(group: Group) {
    // this.filteredPlayers = this.players.filter((p) => console.log(p.memberOf));
    this.filteredPlayers = this.players.filter((p) => p.memberOf.has(group.id));
    this.selectedPlayers = this.filteredPlayers.filter((p) =>
      this.selectedPlayers.includes(p)
    );
  }
}
