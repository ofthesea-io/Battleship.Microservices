import { Component, Input } from '@angular/core';
import { ScoreCard } from '../../../core/models/scoreCard';

@Component({
  selector: 'app-score-card',
  templateUrl: './score-card.component.html',
  styleUrls: ['./score-card.component.css'],
})
export class ScoreCardComponent {
  @Input() scoreCard: ScoreCard;
}
