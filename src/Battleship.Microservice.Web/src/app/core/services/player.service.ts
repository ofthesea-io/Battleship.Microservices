import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Configuration } from '../Utilities/configuration';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Player } from '../models/player';
import { AppConfig } from 'src/app/app.config';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {
  private config: Configuration;

  constructor(private httpClient: HttpClient) {
    this.config = new Configuration();
  }

  createAccount(player: Player): Observable<HttpResponse<any>> {
    const playerUri: string = this.apiServerUrl() + 'createPlayer';
    return this.httpClient.post<any>(playerUri, player, {
      headers: this.config.getHeaders(),
      observe: 'response'
    });
  }

  loginPlayer(player: Player): Observable<HttpResponse<any>> {
    const playerUri: string = this.apiServerUrl() + 'PlayerLogin';
    return this.httpClient.post<any>(playerUri, player, {
      headers: this.config.getHeaders(),
      observe: 'response'
    });
  }

  demoPlayerLogin(playerId: string): Observable<HttpResponse<any>> {
    const playerUri: string = this.apiServerUrl() +  'DemoLogin'.concat('?playerId=' + playerId);
    return this.httpClient.get<any>(playerUri, {
      headers: new HttpHeaders ({
        'Content-Type': 'application/x-www-form-urlencoded'
      }),
      observe: 'response'
    });
  }

  getDemoPlayers(): Observable<HttpResponse<any>> {
    const playerUri: string = this.apiServerUrl() + 'GetDemoPlayers';
    return this.httpClient.get<any>(playerUri, {
      headers: this.config.getHeaders(),
      observe: 'response'
    }).pipe(catchError(this.config.handleError));
  }

  getAuthorization(session: string): Observable<HttpResponse<any>> {
    const authenticationUri: string = this.apiServerUrl() + 'GetAuthorization'.concat('?sessionId=' + session);
    return this.httpClient.get<any>(authenticationUri, {
      headers: this.config.getHeaders(),
      observe: 'response'
    }).pipe(catchError(this.config.handleError));
  }

   apiServerUrl(): string {
    const server: string =  AppConfig.settings.apiServer.Player.host + AppConfig.settings.apiServer.Player.url;
    return server;
  }
}
