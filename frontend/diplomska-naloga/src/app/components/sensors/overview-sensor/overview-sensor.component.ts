import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SensorGroup } from 'src/app/interfaces/sensor-group';
import { SensorGroupService } from 'src/app/services/api/sensor-group.service';

@Component({
  selector: 'app-overview-sensor',
  templateUrl: './overview-sensor.component.html',
  styleUrls: ['./overview-sensor.component.scss']
})
export class OverviewSensorComponent implements OnInit {

  pageNumber = 1;
  pageSize = 12;
  orderDesc = false;
  orderBy = 0;

  sensorGroups :SensorGroup[] = []

  constructor(private serviceSensorGroup:SensorGroupService,
    private router:Router){}
  ngOnInit(): void {
    this.getPaginatedSensorGroup();
  }

  openSensorGroup(sg: SensorGroup){
    console.log("Navigate to sensorGroupID: ", sg.id)
    this.router.navigate(["sensors", sg.id])
  }

  private getPaginatedSensorGroup(){
    this.serviceSensorGroup.getSensorGroupsPagination(this.pageNumber, this.pageSize, this.orderDesc, this.orderBy).subscribe(data => {
      console.log(data)
      this.sensorGroups = data.items
    }, err => console.error(err));
  }

}
