import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Player } from 'src/interfaces/player';

@Injectable({
  providedIn: 'root',
})
export class PlayersService {
  constructor(private httpClient: HttpClient) {}

  getPlayers(): Observable<Player[]> {
    return this.httpClient.get<Player[]>('http://localhost:7000/players');
  }
}
