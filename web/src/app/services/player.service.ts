import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Player } from 'src/interfaces/player';
import { Group } from 'src/interfaces/group';

@Injectable({
  providedIn: 'root',
})
export class PlayerService {
  constructor(private httpClient: HttpClient) {}

  getPlayers(): Observable<Player[]> {
    return this.httpClient.get<Player[]>('http://localhost:7000/players');
  }

  getGroups(): Observable<Group[]> {
    return this.httpClient.get<Group[]>('http://localhost:7000/groups');
  }
}
