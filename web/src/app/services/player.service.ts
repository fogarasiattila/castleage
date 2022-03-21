import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Player } from 'src/interfaces/player';
import { Group } from 'src/interfaces/group';
import { map } from 'rxjs/operators';

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
          result.unshift({ id: 0, name: '<Új csoport>' });
          return result;
        })
      );
  }

  sendGroups(groups: Group[]): Observable<Group[]> {
    return this.httpClient.post<Group[]>(
      'http://localhost/api/Player/Groups',
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
