import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Configuration } from '../helper/configuration';
import { catchError } from 'rxjs/operators';
import { AppConfig } from 'src/app/app.config';
import { AuthenticationService } from './authentication.service';

@Injectable({
    providedIn: 'root',
})
export class ScoreCardService {
    constructor(private httpClient: HttpClient, private config: Configuration, private auth: AuthenticationService) {}

    getPlayerScoreCard(): Observable<HttpResponse<any>> {
        const createScoreCardUrl = this.apiServerUrl() + 'GetPlayerScoreCard';
        return this.httpClient
            .get<any>(createScoreCardUrl,
                {
                    headers: this.auth.getAuthenticationHeaders(),
                    observe: 'response',
                })
            .pipe(catchError(this.config.handleError));
    }

    /* Properties */
    apiServerUrl(): string {
        const server =
            AppConfig.settings.apiServer.ScoreCard.host +
                AppConfig.settings.apiServer.ScoreCard.url;
        return server;
    }
}
