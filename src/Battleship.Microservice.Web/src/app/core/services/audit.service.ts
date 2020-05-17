import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpResponse, HttpClient } from '@angular/common/http';
import { Configuration } from '../utilities/configuration';
import { AppConfig } from 'src/app/app.config';
import { Auth } from '../utilities/auth';

@Injectable({
  providedIn: 'root'
})
export class AuditService {

  constructor(private httpClient: HttpClient, private config: Configuration, private auth : Auth) {
  }

  getAuditContent(): Observable<HttpResponse<any>> {
    const auditLogUrl: string = this.apiServerUrl() + 'GetAuditContent';
    return this.httpClient.get<any>(auditLogUrl, {
        headers: this.auth.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(
        catchError(this.config.handleError));
  }

  getAuditContentByAuditTypeHourRange(auditLogTypeId: number, hours: number): Observable<HttpResponse<any>> {
    const auditLogUrl = `${ this.apiServerUrl() }GetAuditContentByAuditTypeHourRange?auditType=${ auditLogTypeId }&hours=${ hours }`;
    return this.httpClient
      .get<any>(auditLogUrl, {
        headers: this.auth.getAuthHeaders(),
        observe: 'response'
      })
      .pipe(catchError(this.config.handleError));
  }

  /* Properties */
  apiServerUrl(): string {
    const server: string =
      AppConfig.settings.apiServer.AuditLog.host +
      AppConfig.settings.apiServer.AuditLog.url;
    return server;
  }
}
