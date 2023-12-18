import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class StateStorageService {

  private readonly authData = new BehaviorSubject<boolean>(false);

  private readonly userRole = new BehaviorSubject<boolean>(false);
  private readonly userId = new BehaviorSubject<string |  undefined>(undefined);

  private userID:string | undefined = ""


  get getAuthData(){
    return this.authData.asObservable()
  }

  get isUserAdmin(){
    return this.userRole.asObservable()
  }

  get getUserId(){
    return this.userId.asObservable()
  }
  private readonly jwtConstName = "access-token";
  constructor(private router: Router) { }
  saveJwt(jwt: string){
    localStorage.setItem(this.jwtConstName, jwt);
  }

  checkIfJwtSaved(){
    const jwt = localStorage.getItem(this.jwtConstName);
    if (jwt){
      this.authData.next(true);
      const payload = this.getJwtData()
      this.userId.next(payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"])
      const role = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
      if(role === "Admin"){
        this.userRole.next(true)
      }
      else{
        this.userRole.next(false)
      } 
      return true;
    }
    else{
      this.authData.next(false);
      return false;
    }
  }

  removeJwt(){
    localStorage.removeItem(this.jwtConstName);
    this.authData.next(false);
    this.userRole.next(false);
    this.userID = undefined;
    this.userId.next(this.userID);
  }

  getJwt(){
    return localStorage.getItem(this.jwtConstName);
  }

  getJwtData(){
    // Split the JWT token into its parts
    const jwtLocalStorage = this.getJwt();
    if (jwtLocalStorage !== null && jwtLocalStorage !== undefined){
      const parts = jwtLocalStorage.split('.');

      // Decode the base64-encoded payload (second part)
      const base64Url = parts[1];
      const decodedPayload = atob(base64Url);

      const payload = JSON.parse(decodedPayload)

      // Parse the JSON payload
      return payload;
    }
    this.userID = undefined;
    return undefined
  }
}
