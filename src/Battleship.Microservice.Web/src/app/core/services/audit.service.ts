import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpResponse, HttpClient } from '@angular/common/http';
import { Configuration } from '../utilities/configuration';
import { AppConfig } from 'src/app/app.config';
import { AuthenticationService } from './authentication.service';

@Injectable({
    providedIn: 'root'
})
export class AuditService {

    constructor(private httpClient: HttpClient, private config: Configuration, private auth: AuthenticationService) {
    }

    getAuditContent(): Observable<HttpResponse<any>> {
        const auditLogUrl = this.apiServerUrl() + 'GetAuditContent';
        return this.httpClient.get<any>(auditLogUrl,
                {
                    headers: this.auth.getAuthenticationHeaders(),
                    observe: 'response'
                })
            .pipe(
                catchError(this.config.handleError));
    }

    getAuditContentByAuditTypeHourRange(auditLogTypeId: number, hours: number): Observable<HttpResponse<any>> {
        const auditLogUrl =
            `${this.apiServerUrl()}GetAuditContentByAuditTypeHourRange?auditType=${auditLogTypeId}&hours=${hours}`;
        return this.httpClient
            .get<any>(auditLogUrl,
                {
                    headers: this.auth.getAuthenticationHeaders(),
                    observe: 'response'
                })
            .pipe(catchError(this.config.handleError));
    }

    /* Properties */
    apiServerUrl(): string {
        const server =
            AppConfig.settings.apiServer.AuditLog.host +
                AppConfig.settings.apiServer.AuditLog.url;
        return server;
    }
}
