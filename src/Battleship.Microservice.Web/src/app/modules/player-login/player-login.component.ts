import { Component, OnInit } from "@angular/core";
import { Player } from "../../core/models/player";
import { PlayerService } from "../../core/services/player.service";
import { Configuration } from "../../core/utilities/configuration";
import { Router } from "@angular/router";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import HttpStatusCode from "src/app/core/utilities/HttpStatusCodes";


export enum PlayerServiceStatus {
    isLoading = 0,
    loaded = 1,
    unavailable = -1
}

@Component({
    selector: "app-player-login",
    templateUrl: "./player-login.component.html",
    styleUrls: ["./player-login.component.css"]
})
export class PlayerLoginComponent implements OnInit {

    playerServiceStatus: PlayerServiceStatus;

    demoPlayers: Array<Player> = [];
    email: string;
    password: string;
    errorMessage: string;

    constructor(
        private configuration: Configuration,
        private playerService: PlayerService,
        private authenticationService: AuthenticationService,
        private auth: AuthenticationService,
        private router: Router
    ) {
        this.playerServiceStatus = PlayerServiceStatus.isLoading;
    }

    ngOnInit() {
        this.playerServiceStatus = PlayerServiceStatus.isLoading;
        this.getDemoPlayers();
    }

    getDemoPlayers() {
        this.playerService.getDemoPlayers().subscribe(response => {
            if (response.status === HttpStatusCode.OK) {
                this.playerServiceStatus = PlayerServiceStatus.loaded;
                this.demoPlayers = response.body;
            }
        },
            error => {
                this.playerServiceStatus = PlayerServiceStatus.unavailable;
                this.configuration.handleError(error);
            });
    }

    onPlayerDemoLogin(playerId: string) {
        this.authenticationService.demoPlayerLogin(playerId).subscribe(response => {
            if (response.status === HttpStatusCode.OK) {
                const player = response.body as Player;
                player.isDemoAccount = true;
                this.authenticateAndThenRoutePlayer(player);
            }
        },
            error => {
                this.playerServiceStatus = PlayerServiceStatus.unavailable;
                this.configuration.handleError(error);
            }
        );
    }

    onPlayerLogin(data: any) {
        this.authenticationService.playerLogin(data).subscribe(response => {
            if (response.status === HttpStatusCode.OK) {
                const player = response.body as Player;
                player.isDemoAccount = false;
                this.authenticateAndThenRoutePlayer(player);
            }
        },
            error => {
                this.playerServiceStatus = PlayerServiceStatus.unavailable;
                this.configuration.handleError(error);
            }
        );
    }

    private authenticateAndThenRoutePlayer(player: Player): void {
        this.auth.setAuthentication(player);
        this.router.navigate(["gamePlay"], { state: { player } });
    }
}
