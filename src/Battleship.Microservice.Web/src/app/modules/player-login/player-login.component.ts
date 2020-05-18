import { Component, OnInit, Input } from '@angular/core';
import { Player } from '../../core/models/player';
import { PlayerService } from '../../core/services/player.service';
import { Configuration } from '../../core/utilities/configuration';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Auth } from 'src/app/core/utilities/auth';
import HttpStatusCode from 'src/app/core/utilities/HttpStatusCodes';

export enum PlayerServiceStatus {
  isLoading = 0,
  loaded = 1,
  unavailable = -1
}

@Component({
  selector: 'app-player-login',
  templateUrl: './player-login.component.html',
  styleUrls: ['./player-login.component.css']
})
export class PlayerLoginComponent implements OnInit {

  public playerServiceStatus: PlayerServiceStatus;

  demoPlayers: Array<Player> = [];
  email: string;
  password: string;
  errorMessage: string;

  constructor(
    private configuration: Configuration,
    private playerService: PlayerService,
    private auth: Auth,
    private router: Router,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.playerServiceStatus = PlayerServiceStatus.isLoading;
    this.getDemoPlayers();
  }

  getDemoPlayers() {
    this.playerService.getDemoPlayers().subscribe(response => {
      if (response.status === 200) {
        this.playerServiceStatus = PlayerServiceStatus.loaded;
        this.demoPlayers = response.body;
      } else {
        this.playerServiceStatus = PlayerServiceStatus.unavailable;
      }
    },
    error => {
      this.playerServiceStatus = PlayerServiceStatus.unavailable;
      this.configuration.handleError(error);
    });
  }

  onChange(playerId: string) {
    const isCompleted: string = this.auth.getGameCompeted();
    const getDate: string = this.auth.getLastAuthDate();

    if (Date.now() <= Date.parse(getDate) && isCompleted) {
      this.configuration.openDialog(this.dialog, this.configuration.gameNotStarted, this.configuration.close);
    }

    this.playerService.demoPlayerLogin(playerId).subscribe(
      response => {
        console.log(response);
        if (response.status ===  HttpStatusCode.OK) {
          const player = response.body as Player;
          this.auth.setAuthHeader(player.sessionToken);
          this.router.navigate(['gamePlay'], { state: { player }});
        }
      },
      error => {
        this.configuration.handleError(error);
      }
    );
  }

  onSubmit(data) {
    this.playerService.loginPlayer(data).subscribe(response => {
        console.log(response);
        if (response.status === HttpStatusCode.OK) {
          const player: Player = response.body as Player;
          this.auth.setAuthHeader(player.sessionToken);
          this.router.navigate(['gamePlay'], { state: { player } });
        } else {
          this.auth.removeAuthHeader('authToken');
          this.errorMessage = this.configuration.loginFailed;
        }
      },
      error => {
        this.errorMessage = this.configuration.serviceError;
      }
    );
  }
}
