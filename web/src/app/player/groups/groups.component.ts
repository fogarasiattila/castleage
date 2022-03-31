import { ThisReceiver } from '@angular/compiler';
import {
  Component,
  ElementRef,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  NavigationExtras,
  Router,
} from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  GroupEnum,
  _const_newGroupName,
  _const_newGroupNameClean,
} from 'src/app/enums/groupEnum';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { GroupsAndPlayersDto } from 'src/interfaces/groupsAndPlayersDto';
import { Player } from 'src/interfaces/player';
import { GroupsListComponent } from '../groups-list/groups-list.component';

type GroupWithComponentId = { group: Group; compId: number };

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css'],
})
export class GroupsComponent implements OnInit, OnDestroy {
  @ViewChild('group0', { static: true }) group0;
  @ViewChild('group1', { static: true }) group1;
  @ViewChild('groupEdit0', { static: true }) groupEdit0;
  @ViewChild('groupEdit1', { static: true }) groupEdit1;
  groups: Group[] = [null];
  groupNames: string[] = [];
  players: Player[] = [null];
  newGroupRef: number = -1;
  groupSubscription: Subscription;
  playerSubscription: Subscription;

  constructor(
    private playerService: PlayerService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.groupSubscription = this.playerService.groupsState$.subscribe({
      next: (r) => (this.groups = [...r]),
    });
    this.playerSubscription = this.playerService.playersState$.subscribe({
      next: (r) => (this.players = [...r]),
    });
  }

  onMoveLeftToRight(all: boolean) {
    this.onMoveSelected(all, this.group0, this.group1);
  }

  onMoveRightToLeft(all: boolean) {
    this.onMoveSelected(all, this.group1, this.group0);
  }

  onMoveSelected(all: boolean, srcGrp, dstGrp) {
    let change = false;
    let leftSelectedPlayers: Player[] = all
      ? srcGrp.filteredPlayers
      : srcGrp.selectedPlayers;
    if (leftSelectedPlayers.length === 0) return;

    const leftGroupId: number = srcGrp.groupSelected$.getValue().id;
    let rightGroupId: number = dstGrp.groupSelected$.getValue().id;

    const rightGroupName: string =
      rightGroupId !== GroupEnum.NewGroup
        ? dstGrp.groupSelected$.getValue().name
        : _const_newGroupNameClean;

    if (leftGroupId === rightGroupId) return;

    if (rightGroupId === GroupEnum.NewGroup) {
      //ha új csoport, hozzon létre egyet
      rightGroupName;
      var createdGroup = this.createNewGroup(rightGroupName);
      this.groups.push(createdGroup);

      rightGroupId = createdGroup.id;
    }

    leftSelectedPlayers.forEach((selected) => {
      if (leftGroupId !== GroupEnum.Mindenki)
        selected.memberOf.splice(selected.memberOf.indexOf(leftGroupId), 1); //törlés a src group-ból, ha nem "Mindenki"
      selected.touched = true;

      if (
        //ha még nem tagja a csoportnak, adja hozzá
        rightGroupId !== GroupEnum.Mindenki &&
        selected.memberOf.indexOf(rightGroupId) === -1
      ) {
        selected.touched = true;
        // if (createdGroup) selected.memberOfNew.push(createdGroup.name); // ha új csoport, adja hozzá a memberOfNew-hoz
        selected.memberOf.push(rightGroupId);
      }
    });

    if (rightGroupId < GroupEnum.NewGroup && createdGroup) {
      //ha új csoport, akkor váltson át rá
      dstGrp.groupSelected$.next(createdGroup);
    }

    this.playerService.groupsState$.next(this.groups);
  }

  createNewGroup(origName: string) {
    let name = origName;
    let groupNum = 0;

    while (this.groups.some((g) => g.name === name)) {
      //Új csoport számozás növelése, amíg nincs találat
      name = `${origName}${groupNum++}`;
    }
    const group: Group = new Group(this.newGroupRef--, name);
    group.touched = true;

    return group;
  }

  onSave() {
    const tempGroups = this.groups.filter(
      (g) =>
        g.touched && (!g.deleted || (g.deleted && g.id > GroupEnum.Mindenki))
    );
    const tempPlayers = this.players.filter((p) => p.touched);
    if (tempGroups.length === 0 && tempPlayers.length === 0) return;

    const data: GroupsAndPlayersDto = {
      groups: tempGroups,
      players: tempPlayers,
    };
    this.playerService.sendPlayersAndGroups(data);
  }

  onReset() {
    //ehelyett component reset!
    this.playerService.getGroups();
    this.playerService.getPlayers();
  }

  onDeleteGroup(group: Group) {
    if (group.id < GroupEnum.Mindenki) {
      this.groups.splice(this.groups.indexOf(group), 0);
      this.players.forEach((p) => {
        if (p.memberOf.includes(group.id))
          p.memberOf.splice(p.memberOf.indexOf(group.id), 0);
      });
      this.onReset();
    } else {
      this.playerService.deleteGroup(group).subscribe({
        next: (r) => this.onReset(),
        error: (e) => console.log(e.message),
      });
    }
  }

  ngOnDestroy(): void {
    this.groupSubscription.unsubscribe();
    this.playerSubscription.unsubscribe();
  }
}
