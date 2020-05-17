import { Component, OnInit, Input } from '@angular/core';
import { Player } from '../../core/models/player';
import { PlayerService } from '../../core/services/player.service';
import { Configuration } from '../../core/utilities/configuration';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { Auth } from 'src/app/core/utilities/auth';

export enum PlayerDemoStatus {
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

  public playerDemoStatus: PlayerDemoStatus;

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
    this.playerDemoStatus = PlayerDemoStatus.isLoading;
    this.getDemoPlayers();
  }

  getDemoPlayers() {
    this.playerService.getDemoPlayers().subscribe(response => {
      if (response.status === 200) {
        this.playerDemoStatus = PlayerDemoStatus.loaded;
        this.demoPlayers = response.body;
      } else {
        this.playerDemoStatus = PlayerDemoStatus.unavailable;
      }
    },
    error => {
      this.playerDemoStatus = PlayerDemoStatus.unavailable;
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
        if (response.status === 200) {
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
    const player = data as Player;
    this.playerService.loginPlayer(player).subscribe(
      response => {
        console.log(response);
        if (response.status === 200) {
          if (response.body !== '') {
            this.errorMessage = this.configuration.somethingWentWrongError;
          }
          this.auth.setAuthHeader(response.body);
        } else {
          this.auth.removeAuthHeader('authToken');
        }
      },
      error => {
        this.errorMessage = this.configuration.serviceError;
      }
    );
  }
}
