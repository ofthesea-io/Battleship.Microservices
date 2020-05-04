import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerLoginComponent } from './player-login.component';

describe('PlayerLoginComponent', () => {
  let component: PlayerLoginComponent;
  let fixture: ComponentFixture<PlayerLoginComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerLoginComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
