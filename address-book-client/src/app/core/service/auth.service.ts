import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse } from '../models/auth/login-request';
import { RegisterRequest } from '../models/auth/register-request';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7107/api/auth';

  private readonly tokenKey = 'access_token';

  login(request: LoginRequest): Observable<LoginResponse> {

    return this.http.post<LoginResponse>(`${this.apiUrl}/login`,request)
      .pipe(tap(response => {
          localStorage.setItem(
            this.tokenKey,
            response.token
          );
        })
      );
  }


   register(request: RegisterRequest): Observable<void> {
  return this.http.post<void>(`${this.apiUrl}/register`,request
  );
}

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }


 
}