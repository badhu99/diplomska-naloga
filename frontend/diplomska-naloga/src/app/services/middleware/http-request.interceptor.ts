import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { StateStorageService } from '../common/state-storage.service';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {

  constructor(private stateService : StateStorageService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwt = this.stateService.getJwt()
    if(jwt){
      const headers = new HttpHeaders({
        "Content-Type":"application/json",
        "Authorization": `Bearer ${jwt}`,
      })
      request = request.clone({headers : headers});
    }
    return next.handle(request);
  }
}
