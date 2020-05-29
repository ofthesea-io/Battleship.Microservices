import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Configuration } from '../helper/configuration';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AppConfig } from 'src/app/app.config';
import { AuthenticationService } from './authentication.service';

@Injectable({
    providedIn: 'root'
})
export class StatisticsService {

    private config: Configuration;

    constructor(private httpClient: HttpClient, private auth: AuthenticationService) {
        this.config = new Configuration();
    }

    getTopPlayers(): Observable<HttpResponse<any>> {
        const createStatisticsUri = this.apiServerUrl() + 'GetTopTenPlayers';
        return this.httpClient.get<any>(createStatisticsUri,
                {
                    headers: this.auth.getAuthenticationHeaders(),
                    observe: 'response'
                })
            .pipe(
                catchError(this.config.handleError));
    }

    /* Properties */
    apiServerUrl(): string {
        const server = AppConfig.settings.apiServer.Statistics.host + AppConfig.settings.apiServer.Statistics.url;
        return server;
    }
}
