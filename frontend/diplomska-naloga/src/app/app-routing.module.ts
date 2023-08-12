import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizationGuard } from './services/auth/authorization.guard';
import { CheckLoginGuard } from './services/auth/check-login.guard';

const routes: Routes = [
  { path: '', redirectTo: '/sensors', pathMatch: 'full' },
  {
    path: 'auth',
    loadChildren: () =>
      import('./components/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
      canActivate : [CheckLoginGuard]
  },
  {
    path: 'sensors',
    loadChildren: () =>
      import('./components/sensors/sensors.module').then(
        (m) => m.SensorsModule
      ),
      canActivate: [AuthorizationGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
