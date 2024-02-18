import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Router } from '@angular/router';

const AUTH_API = 'https://localhost:7170/Usuarios/';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router) { }

  login(usuario: string, password: string): Observable<any> {
    return this.http.post(
      AUTH_API + 'authenticate',
      {
        usuario,
        password,
      },
      httpOptions
    ).pipe(
      map((data) => {
        this.saveUser(data);
        return data;
      })
    );
  }

  logout() {
    this.clean();
    this.router.navigate(["/login"]);
  }

  clean(): void {
    window.sessionStorage.clear();
  }

  public saveUser(user: any): void {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): any {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return JSON.parse(user);
    }

    return {};
  }

  public isLoggedIn(): boolean {
    const user = window.sessionStorage.getItem(USER_KEY);
    if (user) {
      return true;
    }

    return false;
  }
}