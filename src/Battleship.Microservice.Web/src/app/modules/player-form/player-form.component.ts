import { Component, Input, OnInit } from '@angular/core';
import { Configuration } from '../../core/Utilities/configuration';
import { Player } from '../../core/models/player';
import { PlayerService } from '../../core/services/player.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layer-form-root',
  templateUrl: './player-form.component.html',
  styleUrls: ['./player-form.component.css']
})

export class PlayerFormComponent {
  public errorMessage = '';
  public firstName = '';
  public lastName = '';
  public email = '';
  public password = '';
  public confirmPassword = '';

  constructor(
    private config: Configuration,
    private playerService: PlayerService,
    private router: Router
  ) {}

  onSubmit(data: Player) {
    const player = data as Player;
    this.playerService.createAccount(player).subscribe(
      response => {
        console.log(response);
        if (response.status === 200) {
          if (response.body !== '') {
            this.config.setAuthHeader(response.body.sessionToken);
            this.config.setGameCompleted('no');
            this.router.navigate(['gamePlay'], { state: { player } });
          } else {
            this.errorMessage = this.config.somethingWentWrongError;
          }
        } else {
          this.config.removeAuthHeader('authToken');
          this.errorMessage = this.config.somethingWentWrongError;
        }
      },
      error => {
        this.errorMessage = this.config.serviceError;
      }
    );
  }
}
