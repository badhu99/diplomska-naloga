import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CheckLoginGuard } from './services/auth/check-login.guard';
import { AuthorizationGuard } from './services/auth/authorization.guard';
import { UsersOverviewComponent } from './components/users/users-overview/users-overview.component';

const routes: Routes = [
  { path: '', redirectTo: '/sensors', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () =>
      import('./components/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
      // canActivate : [CheckLoginGuard]
  },
  {
    path: 'sensors',
    loadChildren: () =>
      import('./components/sensors/sensors.module').then(
        (m) => m.SensorsModule
      ),
      // canActivate: [AuthorizationGuard]
  },
  {
    path: 'users',
    component: UsersOverviewComponent,
    canActivate: [AuthorizationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
