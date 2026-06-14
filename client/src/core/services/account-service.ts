import { inject, Injectable, signal } from '@angular/core';
import { User } from '../../types/user';
import { HttpClient } from '@angular/common/http';
import { LoginCreds, RegisterCreds } from '../../types/authentication';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AccountService {

  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);
  private http = inject(HttpClient);

  login(creds: LoginCreds){
    return this.http.post<User>(this.baseUrl  + 'account/login', creds, { withCredentials: true }).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user)
        }
        return user;
      })
    )
  }
  register(creds: RegisterCreds){
    return this.http.post<User>(this.baseUrl + 'account/register', creds, { withCredentials: true }).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }
  setCurrentUser(user: User){
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
  }
  logout(){
    return this.http.post(this.baseUrl + 'account/logout', {}, { withCredentials: true }).pipe(
      map(() => {
        localStorage.removeItem('user');
        this.currentUser.set(null)
      })
    )
  }


}
