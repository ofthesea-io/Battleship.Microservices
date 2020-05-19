import { Component } from '@angular/core';
import { Configuration } from '../../core/utilities/configuration';
import { Player } from '../../core/models/player';
import { PlayerService } from '../../core/services/player.service';
import { Router } from '@angular/router';
import { Auth } from 'src/app/core/utilities/auth';
import HttpStatusCode from 'src/app/core/utilities/HttpStatusCodes';


@Component({
    selector: 'app-layer-form-root',
    templateUrl: './player-form.component.html',
    styleUrls: ['./player-form.component.css']
})
export class PlayerFormComponent {
    errorMessage = '';
    firstName = '';
    lastName = '';
    email = '';
    password = '';
    confirmPassword = '';

    constructor(
        private config: Configuration,
        private auth: Auth,
        private playerService: PlayerService,
        private router: Router
    ) {
    }

    onSubmit(player: Player) {
        this.playerService.createAccount(player).subscribe(
            response => {
                console.log(response);
                if (response.status === HttpStatusCode.OK) {
                    player.playerId = response.body.playerId;
                    this.router.navigate(['login'], { state: { player } });
                } else {
                    this.errorMessage = this.config.somethingWentWrongError;
                }
            },
            error => {
                this.errorMessage = this.config.serviceError;
            }
        );
    }
}
