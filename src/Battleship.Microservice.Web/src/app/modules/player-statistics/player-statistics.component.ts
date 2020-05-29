import { Component, OnInit } from '@angular/core';
import { Player } from '../../core/models/player';
import { Configuration } from '../../core/helper/configuration';
import { StatisticsService } from '../../core/services/statistics.service';

@Component({
    selector: 'app-player-statistics',
    templateUrl: './player-statistics.component.html',
    styleUrls: ['./player-statistics.component.css']
})
export class PlayerStatisticsComponent implements OnInit {
    errorMessage: string;
    players: Array<Player> = [];

    constructor(
        private statisticsService: StatisticsService,
        private config: Configuration
    ) {
    }

    ngOnInit() {
        this.getTopPlayers();
    }

    getTopPlayers() {
        this.statisticsService.getTopPlayers().subscribe(
            data => {
                this.players = data.body;
            },
            error => {
                this.errorMessage = this.config.applicationError;
                console.log(error);
            }
        );
    }
}
