import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';
import {environment} from '../../../environments/environment';
import { AuthenticationService } from 'src/app/core/services/authentication.service';

@Injectable()
export class BasicAuthInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add header with basic auth credentials if user is logged in and request is to the api url
        this.authenticationService.isPlayerAuthenticated().subscribe((player) => {
            const isLoggedIn = player && player.sessionToken;
            const isApiUrl = request.url.startsWith(environment.apiUrl);
            if (isLoggedIn && isApiUrl) {
                request = request.clone({
                    setHeaders: {
                        Authorization: `Basic ${player.sessionToken}`
                    }
                });
            }
        });
        return next.handle(request);
    }
}