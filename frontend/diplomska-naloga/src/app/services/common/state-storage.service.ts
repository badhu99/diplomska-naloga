import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class StateStorageService {

  private readonly authData = new BehaviorSubject<boolean>(false);

  get getAuthData(){
    return this.authData.asObservable()
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
    this.router.navigateByUrl("auth");
  }

  getJwt(){
    return localStorage.getItem(this.jwtConstName);
  }
}
