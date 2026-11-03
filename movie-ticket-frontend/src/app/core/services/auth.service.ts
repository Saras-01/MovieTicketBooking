import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

import { RegisterRequest } from '../../models/auth/register-request.model';
import { LoginRequest } from '../../models/auth/login-request.model';
import { AuthResponse } from '../../models/auth/auth-response.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiUrl = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  // Customer Registration
  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/register`, request)
      .pipe(
        tap(response => this.saveAuthData(response))
      );
  }

  // Customer Login
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => this.saveAuthData(response))
      );
  }

  // Save authentication details
  private saveAuthData(response: AuthResponse): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('user', JSON.stringify({
      userId: response.userId,
      fullName: response.fullName,
      email: response.email,
      role: response.role
    }));
  }

  // Get JWT token
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Get logged-in user
  getCurrentUser(): AuthResponse | null {
    const user = localStorage.getItem('user');

    return user ? JSON.parse(user) as AuthResponse : null;
  }

  // Check login status
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Logout
  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }
}