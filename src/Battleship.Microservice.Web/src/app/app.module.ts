import { APP_INITIALIZER } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { AppConfig } from './app.config';
import { AppComponent } from './app.component';
import { GamingGridComponent } from './modules/gaming-grid/gaming-grid.component';
import { ScoreCardComponent } from './modules/child-components/score-card/score-card.component';
import { Configuration } from './core/helper/configuration';
import { BoardService } from './core/services/board.service';
import { AppRoutingModule, routingComponents } from './app-routing.module';
import { PlayerLoginComponent } from './modules/player-login/player-login.component';
import { ErrorHandlerComponent } from './modules/child-components/error-handler/error-handler.component';
import { ConfirmationDialogComponent } from './modules/child-components/confirmation-dialog/confirmation-dialog.component';
import { ScoreCardService } from './core/services/score-card.service';
import { PlayerService } from './core/services/player.service';
import { PlayerStatisticsComponent } from './modules/player-statistics/player-statistics.component';
import { AuditLogComponent } from './modules/audit-log/audit-log.component';
import { AuditService } from './core/services/audit.service';
import { AuthenticationService } from './core/services/authentication.service';
import { NavigationComponent } from './modules/child-components/navigation/navigation.component';
import { FooterComponent } from './modules/child-components/footer/footer.component';
import { BasicAuthInterceptor } from './core/helper/basic-auth.interceptor';
import { ErrorInterceptor } from './core/helper/error.interceptor';

export function initializeApp(appConfig: AppConfig) {
    return () => appConfig.load();
}

@NgModule({
    declarations: [
        AppComponent,
        GamingGridComponent,
        ScoreCardComponent,
        PlayerLoginComponent,
        routingComponents,
        ErrorHandlerComponent,
        ConfirmationDialogComponent,
        PlayerStatisticsComponent,
        AuditLogComponent,
        NavigationComponent,
        FooterComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        HttpClientModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        BrowserModule,
        BrowserAnimationsModule,
        FontAwesomeModule,
        HttpClientModule,
        AppRoutingModule,
        ReactiveFormsModule,
        MatDialogModule
    ],
    providers: [
        AuthenticationService,
        Configuration,
        BoardService,
        ScoreCardService,
        PlayerService,
        AuditService,
        AppConfig,
        {
            provide: APP_INITIALIZER,
            useFactory: initializeApp,
            deps: [AppConfig],
            multi: true
        },
        { provide: HTTP_INTERCEPTORS, useClass: BasicAuthInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    ],
    bootstrap: [AppComponent],
    entryComponents: [ConfirmationDialogComponent],
})
export class AppModule {
}
