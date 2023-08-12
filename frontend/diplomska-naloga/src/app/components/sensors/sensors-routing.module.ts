import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OverviewSensorComponent } from './overview-sensor/overview-sensor.component';
import { DetailsSensorComponent } from './details-sensor/details-sensor.component';

const routes: Routes = [
  { path: '', component: OverviewSensorComponent },
  { path: ':id', component: DetailsSensorComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class SensorsRoutingModule {}
