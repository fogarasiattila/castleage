import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Player } from 'src/interfaces/player';
import { Group } from 'src/interfaces/group';
import { map } from 'rxjs/operators';
import {
  GroupEnum,
  _const_mindenkiRename,
  _const_newGroupName,
} from '../enums/groupEnum';
import { GroupsComponent } from '../player/groups/groups.component';
import { GroupsAndPlayersDto } from 'src/interfaces/groupsAndPlayersDto';

@Injectable({
  providedIn: 'root',
})
export class PlayerService {
  constructor(private httpClient: HttpClient) {
    this.getPlayers();

    this.getGroups();
  }

  playersState$ = new BehaviorSubject<Player[]>([]);
  groupsState$ = new BehaviorSubject<Group[]>([]);

  getPlayers() {
    this.httpClient
      .get<Player[]>('http://localhost/api/Player/GetPlayers')
      .pipe(
        map((p) => {
          p.forEach((player) => {
            player.displayname =
              player.displayname === null
                ? player.username
                : player.displayname;
          });

          p.sort((a, b) => {
            if (a.displayname.toLowerCase() < b.displayname.toLowerCase())
              return -1;
            if (a.displayname.toLowerCase() > b.displayname.toLowerCase())
              return 1;
            return 0;
          });

          return p;
        })
      )
      .subscribe({
        next: (r) => this.playersState$.next(r),
        error: (e) => console.log(e.message),
      });
  }

  getGroups() {
    this.httpClient
      .get<Group[]>('http://localhost/api/Player/GetGroups')
      .pipe(
        map((result) => {
          const mindenki = result.find((g) => g.id === GroupEnum.Mindenki);
          mindenki.name = _const_mindenkiRename;
          result.sort((a, b) => {
            if (a.name.toLowerCase() < b.name.toLowerCase()) return -1;
            if (a.name.toLowerCase() > b.name.toLowerCase()) return 1;
            return 0;
          });
          result.unshift(new Group(GroupEnum.NewGroup, _const_newGroupName));
          return result;
        })
      )
      .subscribe({
        next: (r) => this.groupsState$.next(r),
        error: (e) => console.log(e.message),
      });
  }

  sendGroup(group: Group): Observable<Group> {
    return this.httpClient.post<Group>(
      'http://localhost/api/Player/AddOrModifyGroup',
      group
    );
  }

  sendGroups(groups: Group[]) {}

  sendPlayersAndGroups(groupsAndPlayers: GroupsAndPlayersDto) {
    this.httpClient
      .post<GroupsAndPlayersDto>(
        'http://localhost/api/Player/Players',
        groupsAndPlayers
      )

      .subscribe({
        next: (result) => {
          this.getGroups();
          this.getPlayers();
        },
        error: (e) => console.log(e.message),
      });
  }

  deleteGroup(group: Group): Observable<object> {
    return this.httpClient.get(
      `http://localhost/api/Player/DeleteGroup?id=${group.id}`
    );
  }
}
