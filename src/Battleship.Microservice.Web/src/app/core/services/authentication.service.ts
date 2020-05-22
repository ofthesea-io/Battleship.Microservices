import { Injectable } from "@angular/core";
import { HttpHeaders, HttpResponse, HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from "rxjs";
import { Player } from "../models/player";
import { AppConfig } from "src/app/app.config";


/* Simple authentication service */
@Injectable()
export class AuthenticationService {

    isAuthenticatedSubject = new BehaviorSubject<boolean>(this.isAuthenticated());

    constructor(private httpClient: HttpClient) { }

    isPlayerAuthenticated(): Observable<boolean> {
        return this.isAuthenticatedSubject.asObservable();
    }

    getAuthenticationHeaders(): HttpHeaders {
        const authorization = sessionStorage.getItem("authToken");
        const authHeaders = new HttpHeaders({
            "Content-Type": "application/json",
            Authorization: authorization
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
        this.removeAuthHeader();
        this.isAuthenticatedSubject.next(false);
    }

    setAuthHeader(sessionToken: string): void {
        sessionStorage.setItem("authToken", sessionToken);
        this.isAuthenticatedSubject.next(true);
    }

    removeAuthHeader(): void {
        sessionStorage.removeItem("authToken");
        this.isAuthenticatedSubject.next(false);
    }

    private isAuthenticated(): boolean {
        let result = false;
        const authorization = sessionStorage.getItem("authToken");
        if (authorization) {
            result = true;
        }
        return result;
    }

    private apiServerUrl(): string {
        const server = AppConfig.settings.apiServer.Player.host + AppConfig.settings.apiServer.Player.url;
        return server;
    }
}

