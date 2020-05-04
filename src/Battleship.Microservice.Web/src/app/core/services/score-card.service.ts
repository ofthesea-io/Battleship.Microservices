import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {HttpClient, HttpResponse} from '@angular/common/http';
import { Configuration } from '../Utilities/configuration';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ScoreCardService {

  private config: Configuration;
  private host = 'http://localhost:8083';
  private scoreCardUrl: string = this.host + '/api/ScoreCard/';

  constructor(private httpClient: HttpClient) {
      this.config = new Configuration();
   }

  getPlayerScoreCard(): Observable<HttpResponse<any>> {
    const createScoreCardUrl: string = this.scoreCardUrl + 'GetPlayerScoreCard';
    return this.httpClient.get<any>(createScoreCardUrl, {
        headers: this.config.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(
        catchError(this.config.handleError));
  }
}
