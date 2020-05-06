import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs/Subject';
import { Player } from '../models/player';
import { Configuration } from '../Utilities/configuration';
import { catchError } from 'rxjs/operators';
import { PlayerCommand } from '../models/playerCommand';
import 'rxjs/add/operator/map';
import { AppConfig } from 'src/app/app.config';

@Injectable({
  providedIn: 'root'
})
export class BoardService {
  private config: Configuration;
  public gamingMessage = new Subject<string>();
  public gamingGridSubject = new Subject<any>();
  public gamingGridNewGameSubject = new Subject<any>();
  public playerSubject = new Subject<Player>();

  constructor(private httpClient: HttpClient) {
    this.config = new Configuration();
  }

  /* Start the game */
  startGame(numberOfShips: number): Observable<HttpResponse<any>> {
    const startGameUri = this.apiServerUrl() + 'StartGame'.concat('?numberOfShips=' + numberOfShips);
    return this.httpClient
      .get<any>(startGameUri, {
        headers: this.config.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(catchError(this.config.handleError));
  }

  /* Get the gaming grid */
  getGamingGrid(): Observable<HttpResponse<any>> {
    const getGamingGridUrl = this.apiServerUrl() + 'GenerateBoard';
    return this.httpClient
      .get<any>(getGamingGridUrl, {
        headers: this.config.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(catchError(this.config.handleError));
  }

  /* Send the user command */
  UserInput(playerCommand: PlayerCommand): Observable<HttpResponse<any>> {
    if (playerCommand.coordinate.y <= 0 && playerCommand.coordinate.x <= 0) {
      this.gamingGridSubject.next(this.config.incorrect);
    } else {
      const userInputUrl: string = this.apiServerUrl() + 'UserInput';
      return this.httpClient
        .post<any>(userInputUrl, playerCommand, {
          headers: this.config.getAuthHeaders(),
          observe: 'response'
        })
        .pipe(catchError(this.config.handleError));
    }
  }

  /* Properties */
   apiServerUrl(): string {
    const server: string =  AppConfig.settings.apiServer.Board.host + AppConfig.settings.apiServer.Board.url;
    return server;
  }
}
