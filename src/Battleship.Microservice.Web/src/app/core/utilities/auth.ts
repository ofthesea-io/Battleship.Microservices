import { HttpHeaders } from '@angular/common/http';

export class Auth {

    getHeaders(): HttpHeaders {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
        return headers;
    }

    getAuthHeaders(): HttpHeaders {
        const authorization = sessionStorage.getItem('authToken');
        const authHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: authorization
        });
        return authHeaders;
    }

    isAuthenticated(): boolean {
        let result = false;
        const authorization = sessionStorage.getItem('authToken');
        if (authorization) {
            result = true;
        }
        return result;
    }

    setAuthHeader(sessionToken: string): void {
        sessionStorage.setItem('authDate', Date.now().toString());
        sessionStorage.setItem('authToken', sessionToken);
    }

    removeAuthHeader(): void {
        sessionStorage.removeItem('authDate');
        sessionStorage.removeItem('authToken');
    }

    setGameCompleted(isCompleted: string) {
        sessionStorage.setItem('isCompleted', isCompleted);
    }

    getGameCompeted(): string {
        return sessionStorage.getItem('isCompleted');
    }

    getLastAuthDate(): string {
        return sessionStorage.getItem('authDate');
    }
}
