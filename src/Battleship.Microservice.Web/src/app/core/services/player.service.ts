import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Configuration } from '../helper/configuration';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Player } from '../models/player';
import { AppConfig } from 'src/app/app.config';
import { AuthenticationService } from './authentication.service';

@Injectable({
    providedIn: 'root'
})
export class PlayerService {
    private config: Configuration;

    constructor(private httpClient: HttpClient, private auth: AuthenticationService) {
        this.config = new Configuration();
    }

    createAccount(player: Player): Observable<HttpResponse<any>> {
        const playerUri = this.playApiServerUrl() + 'createPlayer';
        return this.httpClient.post<any>(playerUri,
            player,
            {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                observe: 'response'
            });
    }

    getDemoPlayers(): Observable<HttpResponse<any>> {
        const playerUri = this.playApiServerUrl() + 'GetDemoPlayers';
        return this.httpClient.get<any>(playerUri,
            {
                headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
                observe: 'response'
            }).pipe(catchError(this.config.handleError));
    }

    private playApiServerUrl(): string {
        const server = AppConfig.settings.apiServer.Player.host + AppConfig.settings.apiServer.Player.url;
        return server;
    }
}
