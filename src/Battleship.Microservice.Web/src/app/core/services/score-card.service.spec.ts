import { TestBed } from '@angular/core/testing';

import { ScoreCardService } from './score-card.service';

describe('ScoreCardService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ScoreCardService = TestBed.get(ScoreCardService);
    expect(service).toBeTruthy();
  });
});
