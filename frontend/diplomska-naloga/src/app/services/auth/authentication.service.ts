import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/app/environment/environment';
import { AuthUserResponse } from 'src/app/interfaces/auth-user-response';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  endpoint = `${environment.apiUrl}/api/Authentication`
  constructor(private http: HttpClient) { }

  SignIn(username:string, password:string) : Observable<AuthUserResponse>{

    const url = `${this.endpoint}/SignIn`
    const body = {
      username,
      password
    }

    return this.http.post<AuthUserResponse>(url, body);
  }

  SignUp(firstname:string, lastname:string, email:string,
    username:string, password:string):Observable<AuthUserResponse>{


      const url = `${this.endpoint}/SignUp`
      const body = {
        username,
        password,
        firstname,
        lastname,
        email
      }
  
      return this.http.post<AuthUserResponse>(url, body);
    }

}
