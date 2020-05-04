import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Configuration } from '../Utilities/configuration';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private config: Configuration;
  private host = 'http://localhost:8084';
  private statisticsUrl: string = this.host + '/api/Statistics/';

  constructor(private httpClient: HttpClient) {
      this.config = new Configuration();
   }

  getTopPlayers(): Observable<HttpResponse<any>> {
    const createStatisticsUri: string = this.statisticsUrl + 'GetTopPlayers';
    return this.httpClient.get<any>(createStatisticsUri, {
        headers: this.config.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(
        catchError(this.config.handleError));
  }
}
