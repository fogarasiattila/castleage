import {
  Component,
  ElementRef,
  OnChanges,
  OnInit,
  SimpleChanges,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { PlayerService } from 'src/app/services/player.service';
import { Group } from 'src/interfaces/group';
import { Player } from 'src/interfaces/player';

@Component({
  selector: 'app-edit-groups',
  templateUrl: './edit-groups.component.html',
  styleUrls: ['./edit-groups.component.css'],
})
export class EditGroupsComponent implements OnInit {
  @ViewChild('group0', { static: true }) group0;
  @ViewChild('group1', { static: true }) group1;
  // groups$ = new BehaviorSubject<Group[]>([]);
  // players$ = new BehaviorSubject<Player[]>([]);
  groups: Group[] = [null];
  players: Player[] = [null];
  // filteredPlayers: Player[] = [];
  // selectedPlayers: Player[] = [];

  constructor(private playerService: PlayerService) {}

  ngOnInit(): void {
    this.playerService.getGroups().subscribe((r) => (this.groups = r));
    this.playerService
      .getPlayers()
      .pipe(
        //Set-é alakítjuk a JSON array-t
        map((players) => {
          players.forEach((p) => {
            p.memberOf = new Set(p.memberOf);
          });
          return players;
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
    const leftSelectedPlayers: Player[] = all
      ? srcGrp.filteredPlayers
      : srcGrp.selectedPlayers;
    if (leftSelectedPlayers.length === 0) return;
    const leftGroupId: number = srcGrp.form.get('groupFilter').value.id;
    const rightGroupId: number = dstGrp.form.get('groupFilter').value.id;
    if (leftGroupId === rightGroupId) return;
    const refreshedPlayers = [...this.players];
    leftSelectedPlayers.forEach((selected) => {
      if (leftGroupId !== 0) selected.memberOf.delete(leftGroupId);
      selected.memberOf.add(rightGroupId);
      refreshedPlayers.map((p) => {
        if (selected.id === p.id) p.memberOf = selected.memberOf;
      });
    });
    this.players = refreshedPlayers;
  }
}
