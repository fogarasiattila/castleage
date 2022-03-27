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
  OnDestroy,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatListOption } from '@angular/material/list';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { GroupEnum, _const_mindenkiRename } from 'src/app/enums/groupEnum';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

type GroupWithComponentId = { group: Group; compId: number };

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.css'],
})
export class GroupsListComponent implements OnInit, OnDestroy, OnChanges {
  groups: Group[] = [];
  nonDeletedGroups: Group[] = [];
  players: Player[] = [];

  @Input() compId: number;

  filteredPlayers: Player[] = [];
  selectedPlayers: Player[] = [];

  form: FormGroup = new FormGroup({
    groupFilter: new FormControl(),
  });

  set groupFilter(value: Group) {
    this.form.patchValue({ groupFilter: value });
  }
  get groupFilter(): Group {
    return this.form.get('groupFilter').value;
  }

  groupSubscription: Subscription;
  playerSubscription: Subscription;
  groupSelected$ = new BehaviorSubject<Group>(
    new Group(GroupEnum.Mindenki, _const_mindenkiRename)
  );

  constructor(private playerService: PlayerService) {}

  ngOnChanges(changes: SimpleChanges): void {}

  ngOnInit(): void {
    this.groupSelected$.subscribe({
      next: (g) => {
        this.groupFilter = g;
      },
    });

    this.groupSubscription = this.playerService.groupsState$.subscribe({
      next: (r) => {
        if (r.length === 0) return;
        this.groups = [...r];
        this.nonDeletedGroups = this.groups.filter((g) => !g.deleted);

        const choosenGroup: Group = this.groupSelected$.getValue()
          ? this.groupSelected$.getValue()
          : r.find((v) => v.id === GroupEnum.Mindenki);
        this.groupSelected$.next(choosenGroup);
        this.groupSelected$.next(choosenGroup);
        this.filteredPlayers = this.players.filter((p) =>
          p.memberOf.includes(choosenGroup.id)
        );
      },
    });

    this.playerSubscription = this.playerService.playersState$.subscribe({
      next: (r) => {
        if (r.length === 0) return;
        this.players = [...r];
        this.filteredPlayers = r.filter(
          (p) => p.memberOf.indexOf(this.groupSelected$.getValue().id) !== -1
        );
        this.selectedPlayers = []; //Töröljük a selection-öket
      },
    });
  }

  onPlayerSelected(options: MatListOption[]) {}

  onGroupSelection(group: Group) {
    this.selectedPlayers = [];
    this.filteredPlayers = this.players.filter(
      (p) => p.memberOf.indexOf(group.id) !== -1
    );
    this.groupSelected$.next(group);
  }

  onGroupDelete(group: Group) {
    if (group.id === GroupEnum.Mindenki || group.id === GroupEnum.NewGroup)
      return;

    this.players.forEach((p) => {
      if (p.memberOf.includes(group.id)) {
        p.memberOf.splice(p.memberOf.indexOf(group.id), 1);
        p.touched = true;
      }
    });

    // this.groups.splice(this.groups.indexOf(group), 1);
    this.playerService.groupsState$.next(this.groups);
    this.playerService.playersState$.next(this.players);
    this.onGroupSelection(this.groups.find((g) => g.id === GroupEnum.Mindenki));
  }

  ngOnDestroy(): void {
    this.groupSubscription.unsubscribe();
    this.playerSubscription.unsubscribe();
  }
}
