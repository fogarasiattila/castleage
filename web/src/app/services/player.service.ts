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

@Injectable({
  providedIn: 'root',
})
export class PlayerService {
  constructor(private httpClient: HttpClient) {}

  getPlayers(): Observable<Player[]> {
    return this.httpClient.get<Player[]>(
      'http://localhost/api/Player/GetPlayers'
    );
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
          result.unshift({ id: 0, name: _const_newGroupName });
          return result;
        })
      );
  }

  sendGroup(groups: Group): Observable<Group> {
    return this.httpClient.post<Group>(
      'http://localhost/api/Player/AddOrModifyGroup',
      groups
    );
  }

  sendPlayers(players: Player[]): Observable<Player[]> {
    return this.httpClient.post<Player[]>(
      'http://localhost/api/Player/Players',
      players
    );
  }
}
