import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SensorDetailsService } from 'src/app/services/api/sensor-details.service';

@Component({
  selector: 'app-details-sensor',
  templateUrl: './details-sensor.component.html',
  styleUrls: ['./details-sensor.component.scss']
})
export class DetailsSensorComponent implements OnInit {
  sensorGroupId = ""
  pageNumber = 1
  pageSize = 12
  sensorsData: any

  constructor(private activatedRoute: ActivatedRoute,
    private sensorDetailsService: SensorDetailsService){}
  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.sensorGroupId = params["id"]
      this.sensorDetailsService.getSensorData(this.sensorGroupId, this.pageSize, this.pageNumber).subscribe(data => {
        console.log(data)
        console.log(data.length)
        this.sensorsData = data
      })
    })
  }
}
