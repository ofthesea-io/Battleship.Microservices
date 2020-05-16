import { HttpHeaders } from '@angular/common/http';

export class Authentication {

    getHeaders(): HttpHeaders {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json'
        });
        return headers;
    }

    getAuthHeaders(): HttpHeaders {
        const authorization = localStorage.getItem('authToken');
        const authHeaders = new HttpHeaders({
            'Content-Type': 'application/json',
            Authorization: authorization
        });
        return authHeaders;
    }

    setAuthHeader(sessionToken: string): void {
        localStorage.setItem('authDate', Date.now().toString());
        localStorage.setItem('authToken', sessionToken);
    }

    removeAuthHeader(sessionToken: string): void {
        localStorage.removeItem('authDate');
        localStorage.removeItem('authToken');
    }

    setGameCompleted(isCompleted: string) {
        localStorage.setItem('isCompleted', isCompleted);
    }

    getGameCompeted(): string {
        return localStorage.getItem('isCompleted');
    }

    getLastAuthDate(): string {
        return localStorage.getItem('authDate');
    }
}
