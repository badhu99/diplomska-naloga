import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpRequestInterceptor } from './services/middleware/http-request.interceptor';
import { UsersOverviewComponent } from './components/users/users-overview/users-overview.component';

@NgModule({
  declarations: [
    AppComponent,
    UsersOverviewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: HttpRequestInterceptor,
    multi: true,     
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
