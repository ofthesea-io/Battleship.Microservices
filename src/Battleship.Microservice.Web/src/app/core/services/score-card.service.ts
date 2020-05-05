import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {HttpClient, HttpResponse} from '@angular/common/http';
import { Configuration } from '../Utilities/configuration';
import { catchError } from 'rxjs/operators';
import { AppConfig } from 'src/app/app.config';

@Injectable({
  providedIn: 'root'
})
export class ScoreCardService {

  private config: Configuration;

  constructor(private httpClient: HttpClient) {
      this.config = new Configuration();
   }

  getPlayerScoreCard(): Observable<HttpResponse<any>> {
    const createScoreCardUrl: string = this.apiServerUrl() + 'GetPlayerScoreCard';
    return this.httpClient.get<any>(createScoreCardUrl, {
        headers: this.config.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(
        catchError(this.config.handleError));
  }

     /* Properties */
     apiServerUrl(): string {
      const server: string =  AppConfig.settings.apiServer.ScoreCard.host + AppConfig.settings.apiServer.ScoreCard.url;
      return server;
    }
}
