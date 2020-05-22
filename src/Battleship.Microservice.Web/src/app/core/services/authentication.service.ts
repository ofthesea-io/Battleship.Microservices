import { Injectable } from "@angular/core";
import { HttpHeaders, HttpResponse, HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { Player } from "../models/player";
import { AppConfig } from "src/app/app.config";
import { PlayerCommand } from '../models/playerCommand';


/* Simple authentication service */
@Injectable()
export class AuthenticationService {

    isAuthenticatedSubject = new BehaviorSubject<Player>(this.isAuthenticated());

    constructor(private httpClient: HttpClient) { }

    isPlayerAuthenticated(): Observable<Player> {
        return this.isAuthenticatedSubject.asObservable();
    }

    getAuthenticationHeaders(): HttpHeaders {
        const authorization = sessionStorage.getItem("authentication");
        let token: string;
        if (authorization != null) {
            const player: Player =  JSON.parse(authorization);
            token = player.sessionToken;
        }
        const authHeaders = new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: token
        });
        return authHeaders;
    }

    playerLogin(player: Player): Observable<HttpResponse<any>> {
        const playerUri = this.apiServerUrl() + "PlayerLogin";
        return this.httpClient.post<any>(playerUri, player,
            {
                headers: new HttpHeaders({ "Content-Type": "application/json" }),
                observe: "response"
            });
    }

    demoPlayerLogin(playerId: string): Observable<HttpResponse<any>> {
        const playerUri = this.apiServerUrl() + "DemoLogin".concat(`?playerId=${playerId}`);
        return this.httpClient.get<any>(playerUri,
            {
                headers: new HttpHeaders({
                    "Content-Type": "application/x-www-form-urlencoded"
                }),
                observe: "response"
            });
    }

    logout(): void {
        this.removeAuthentication();
        this.isAuthenticatedSubject.next(null);
    }

    setAuthentication(session: Player): void {
        if (session !== null) {
            const result = JSON.stringify(session);
            sessionStorage.setItem("authentication", result);
            this.isAuthenticatedSubject.next(session);
        }
    }

    removeAuthentication(): void {
        sessionStorage.removeItem("authentication");
        this.isAuthenticatedSubject.next(null);
    }

    private isAuthenticated(): Player {
        let result: Player = null;
        const authorization = sessionStorage.getItem("authentication");
        if (authorization !== null) {
            result =  JSON.parse(authorization);
        }
        return result;
    }

    private apiServerUrl(): string {
        const server = AppConfig.settings.apiServer.Player.host + AppConfig.settings.apiServer.Player.url;
        return server;
    }
}

