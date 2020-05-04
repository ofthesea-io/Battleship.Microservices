import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Configuration } from '../Utilities/configuration';
import { Observable } from 'rxjs';
import { Player } from '../models/player';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
  private config: Configuration;
  private host = 'http://localhost:8081';
  private playerUrl: string = this.host + '/api/Player/';

  constructor(private httpClient: HttpClient) {
    this.config = new Configuration();
  }

  createAccount(player: Player): Observable<HttpResponse<any>> {
    const playerUri: string = this.playerUrl + 'createPlayer';
    return this.httpClient.post<any>(playerUri, player, {
      headers: this.config.getHeaders(),
      observe: 'response'
    });
  }

  loginPlayer(player: Player): Observable<HttpResponse<any>> {
    const playerUri: string = this.playerUrl + 'PlayerLogin';
    return this.httpClient.post<any>(playerUri, player, {
      headers: this.config.getHeaders(),
      observe: 'response'
    });
  }

  demoPlayerLogin(playerId: string): Observable<HttpResponse<any>> {
    const playerUri: string = this.playerUrl +  'DemoLogin'.concat('?playerId=' + playerId);
    return this.httpClient.get<any>(playerUri, {
      headers: new HttpHeaders ({
        'Content-Type': 'application/x-www-form-urlencoded'
      }),
      observe: 'response'
    });
  }

  getDemoPlayers(): Observable<HttpResponse<any>> {
    const playerUri: string = this.playerUrl + 'GetDemoPlayers';
    return this.httpClient.get<any>(playerUri, {
      headers: this.config.getHeaders(),
      observe: 'response'
    });
  }
}
