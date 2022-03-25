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
import { GroupEnum } from 'src/app/enums/groupEnum';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

type GroupWithComponentId = { group: Group; compId: number };

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit, OnChanges {
  _groups: Group[] = [];
  _players: Player[] = [];

  @Output() groupSelected = new EventEmitter<GroupWithComponentId>();

  @Input()
  set groups(value: Group[]) {
    if (value[0] === null) return;
    this._groups = value;

    const mindenki = value.find((v) => v.id === GroupEnum.Mindenki);
    this.form.patchValue({ groupFilter: mindenki });
    this.filteredPlayers = this.players;
  }
  get groups() {
    return this._groups;
  }

  @Input()
  set players(value) {
    if (value[0] === null) return;
    value.sort((a, b) => {
      if (a.displayname.toLowerCase() < b.displayname.toLowerCase()) return -1;
      if (a.displayname.toLowerCase() > b.displayname.toLowerCase()) return 1;
      return 0;
    });

    this._players = value;
    this.filteredPlayers = value.filter(
      (p) => p.memberOf.indexOf(this.form.get('groupFilter').value.id) !== -1
    );
    this.selectedPlayers = []; //Töröljük a selection-öket
  }
  get players() {
    return this._players.sort();
  }

  @Input() compId: number;

  filteredPlayers: Player[] = [];
  selectedPlayers: Player[] = [];

  form: FormGroup = new FormGroup({
    groupFilter: new FormControl([]),
  });

  constructor(private playerService: PlayerService) {}

  ngOnChanges(changes: SimpleChanges): void {}

  ngOnInit(): void {}

  onPlayerSelected(options: MatListOption[]) {}

  onGroupSelection(group: Group) {
    this.selectedPlayers = [];
    this.filteredPlayers = this.players.filter(
      (p) => p.memberOf.indexOf(group.id) !== -1
    );
    // this.selectedPlayers = this.filteredPlayers.filter((p) =>
    //   this.selectedPlayers.includes(p)
    // );
    this.groupSelected.emit({ compId: this.compId, group });
  }
}
