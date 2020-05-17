import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Configuration } from '../utilities/configuration';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AppConfig } from 'src/app/app.config';
import { Auth } from '../utilities/auth';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private config: Configuration;

  constructor(private httpClient: HttpClient, private auth: Auth) {
      this.config = new Configuration();
   }

  getTopPlayers(): Observable<HttpResponse<any>> {
    const createStatisticsUri: string = this.apiServerUrl() + 'GetTopPlayers';
    return this.httpClient.get<any>(createStatisticsUri, {
        headers: this.auth.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(
        catchError(this.config.handleError));
  }

     /* Properties */
     apiServerUrl(): string {
      const server: string =  AppConfig.settings.apiServer.Player.host + AppConfig.settings.apiServer.Player.url;
      return server;
    }
}
