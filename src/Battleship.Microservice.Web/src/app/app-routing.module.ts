import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PlayerFormComponent } from './modules/player-form/player-form.component';
import { GamingGridComponent } from './modules/gaming-grid/gaming-grid.component';
import { PlayerLoginComponent } from './modules/player-login/player-login.component';
import { PlayerStatisticsComponent } from './modules/player-statistics/player-statistics.component';
import { AuditLogComponent } from './modules/audit-log/audit-log.component';

const routes: Routes = [
  { path: 'createPlayer', component: PlayerFormComponent },
  { path: 'gamePlay', component: GamingGridComponent },
  { path: 'login', component: PlayerLoginComponent },
  { path: 'statistics', component: PlayerStatisticsComponent },
  { path: 'audit', component: AuditLogComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
export const routingComponents = [
  PlayerFormComponent,
  GamingGridComponent,
  PlayerLoginComponent,
  PlayerStatisticsComponent
];
