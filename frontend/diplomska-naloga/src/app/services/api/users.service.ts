import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/app/environment/environment';
import { AuthUserResponse } from 'src/app/interfaces/auth-user-response';

@Injectable({
  providedIn: 'root'
})
export class UsersService {


  endpoint = `${environment.apiUrl}/api/User`
  constructor(private http:HttpClient) { }
  getSensorGroupsPagination():Observable<AuthUserResponse[]>{
    const url = `${this.endpoint}/GetUsers`
    return this.http.get<AuthUserResponse[]>(url);
  }

  activateUser(userId: string, activate: boolean):Observable<void>{
    const url = `${this.endpoint}/Activate/${userId}`;
    const body = {
      IsActive: activate
    }    
    return this.http.post<void>(url, body);
  }
}
