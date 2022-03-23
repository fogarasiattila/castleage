import { ThisReceiver } from '@angular/compiler';
import {
  Component,
  ElementRef,
  OnChanges,
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
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { GroupEnum } from 'src/app/enums/groupEnum';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

type GroupWithComponentId = { group: Group; compId: number };
type GroupRename = { srcGrp: number; dstGrp: Group; compId: number };

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css'],
})
export class GroupsComponent implements OnInit {
  @ViewChild('group0', { static: true }) group0;
  @ViewChild('group1', { static: true }) group1;
  @ViewChild('groupEdit0', { static: true }) groupEdit0;
  @ViewChild('groupEdit1', { static: true }) groupEdit1;
  groups: Group[] = [null];
  players: Player[] = [null];
  // intoGroup: Group;

  constructor(
    private playerService: PlayerService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.refreshGroups();
    this.refreshPlayers();
  }

  refreshGroups() {
    this.playerService.getGroups().subscribe((r) => (this.groups = r));
  }

  refreshPlayers() {
    this.playerService
      .getPlayers()
      .pipe(
        map((p) => {
          p.forEach((player) => {
            player.displayname =
              player.displayname === null
                ? player.username
                : player.displayname;
          });
          return p;
        })
      )
      .subscribe((r) => (this.players = r));
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
    const leftGroupId: number = srcGrp.form.get('groupFilter').value.id;
    const rightGroupId: number = dstGrp.form.get('groupFilter').value.id;
    if (leftGroupId === rightGroupId) return;
    const changeRef = [...this.players]; //új referencia az Angular change detection okán
    leftSelectedPlayers.forEach((selected) => {
      if (leftGroupId !== 1)
        selected.memberOf.splice(selected.memberOf.indexOf(leftGroupId), 1); //törlés a src group-ból, ha nem "Mindenki"
      if (
        rightGroupId === 1 ||
        selected.memberOf.indexOf(rightGroupId) === -1
      ) {
        change = true;
        selected.touched = true;
        selected.memberOf.push(rightGroupId); //ha még nem tagja a csoportnak, adja hozzá
      }
    });
    if (change) {
      // this.intoGroup = this.groups.find((g) => g.id === rightGroupId);
      this.players = changeRef;
      // console.log(this.players);
    }
  }

  onGroupSelected(event: GroupWithComponentId) {
    if (event.compId === 0) this.groupEdit0.groupSelected = event.group;
    else if (event.compId === 1) this.groupEdit1.groupSelected = event.group;
  }

  onGroupRename(event: GroupRename) {
    if (event.srcGrp !== event.dstGrp.id) {
      const playersToChangeIdZero =
        event.compId === 0
          ? this.group0.filteredPlayers
          : this.group1.filteredPlayers;

      playersToChangeIdZero.forEach((p) => {
        const idx = p.memberOf.indexOf(GroupEnum.NewGroup);
        p.memberOf[idx] = event.dstGrp;
      });
    }

    this.refreshGroups();

    event.compId === 0
      ? this.group0.form.patchValue({ groupFilter: event.dstGrp })
      : this.group0.form.patchValue({ groupFilter: event.dstGrp });
  }

  onSave() {
    const tempPlayers = this.players.filter((p) => p.touched);
    this.playerService.sendPlayers(tempPlayers).subscribe({
      next: (result) => {
        console.log(result);
        this.refreshPlayers();
      },
      error: (e) => console.log(e.message),
    });
  }

  onReset() {
    this.refreshGroups();
    this.refreshPlayers();
    this.onGroupSelected({ compId: 0, group: this.group0 });
    this.onGroupSelected({ compId: 1, group: this.group1 });
  }
}
