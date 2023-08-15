import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { SensorsRoutingModule } from './sensors-routing.module';
import { DetailsSensorComponent } from './details-sensor/details-sensor.component';
import { OverviewSensorComponent } from './overview-sensor/overview-sensor.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    DetailsSensorComponent,
    OverviewSensorComponent
  ],
  imports: [
    CommonModule,
    SensorsRoutingModule,
    NgxChartsModule,
    ReactiveFormsModule
  ],
  providers:[
    DatePipe
  ]
})
export class SensorsModule { }
