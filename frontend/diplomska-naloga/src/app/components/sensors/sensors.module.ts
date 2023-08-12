import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SensorsRoutingModule } from './sensors-routing.module';
import { DetailsSensorComponent } from './details-sensor/details-sensor.component';
import { OverviewSensorComponent } from './overview-sensor/overview-sensor.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';


@NgModule({
  declarations: [
    DetailsSensorComponent,
    OverviewSensorComponent
  ],
  imports: [
    CommonModule,
    SensorsRoutingModule,
    NgxChartsModule
  ]
})
export class SensorsModule { }
