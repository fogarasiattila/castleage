import {
  Component,
  ElementRef,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

@Component({
  selector: 'app-edit-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css'],
})
export class GroupsComponent implements OnInit {
  @ViewChild('group0', { static: true }) group0;
  @ViewChild('group1', { static: true }) group1;
  groups: Group[] = [null];
  players: Player[] = [null];

  constructor(
    private playerService: PlayerService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.playerService.getGroups().subscribe((r) => (this.groups = r));
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
    if (change) this.players = changeRef;
  }

  onSave() {
    const tempPlayers = this.players.filter((p) => p.touched);
    this.playerService.sendPlayers(tempPlayers).subscribe({
      next: (result) => console.log(result),
    });
  }

  onReset() {
    this.router.navigate([this.router.url]);
  }
}
