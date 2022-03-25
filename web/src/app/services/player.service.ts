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
  constructor(private httpClient: HttpClient) {}

  getPlayers(): Observable<Player[]> {
    return this.httpClient
      .get<Player[]>('http://localhost/api/Player/GetPlayers')
      .pipe();
  }

  getGroups(): Observable<Group[]> {
    return this.httpClient
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
          result.unshift({ id: 0, name: _const_newGroupName, touched: false });
          return result;
        })
      );
  }

  sendGroup(group: Group): Observable<Group> {
    return this.httpClient.post<Group>(
      'http://localhost/api/Player/AddOrModifyGroup',
      group
    );
  }

  sendGroups(groups: Group[]) {}

  sendPlayers(
    groupsAndPlayers: GroupsAndPlayersDto
  ): Observable<GroupsAndPlayersDto> {
    return this.httpClient.post<GroupsAndPlayersDto>(
      'http://localhost/api/Player/Players',
      groupsAndPlayers
    );
  }

  deleteGroup(group: Group): Observable<object> {
    return this.httpClient.get(
      `http://localhost/api/Player/DeleteGroup?id=${group.id}`
    );
  }
}
