import { TestBed } from '@angular/core/testing';

import { BoardService } from './board.service';

describe('BattleshipService',
    () => {
        beforeEach(() => TestBed.configureTestingModule({}));

        it('should be created',
            () => {
                const service: BoardService = TestBed.inject(BoardService);
                expect(service).toBeTruthy();
            });
    });
