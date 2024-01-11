import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { SensorsRoutingModule } from './sensors-routing.module';
import { DetailsSensorComponent } from './details-sensor/details-sensor.component';
import { OverviewSensorComponent } from './overview-sensor/overview-sensor.component';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { ReactiveFormsModule } from '@angular/forms';


import { MatDatepickerModule } from '@angular/material/datepicker';


@NgModule({
  declarations: [
    DetailsSensorComponent,
    OverviewSensorComponent,
  ],
  imports: [
    CommonModule,
    SensorsRoutingModule,
    NgxChartsModule,
    ReactiveFormsModule,
    MatDatepickerModule
  ],
  providers:[
    DatePipe
  ]
})
export class SensorsModule { }
