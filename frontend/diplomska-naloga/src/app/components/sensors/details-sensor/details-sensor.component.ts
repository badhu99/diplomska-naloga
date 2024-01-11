import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ISensorDetails } from 'src/app/interfaces/sensor-details';
import { SensorDetailsService } from 'src/app/services/api/sensor-details.service';
import { StateStorageService } from 'src/app/services/common/state-storage.service';

@Component({
  selector: 'app-details-sensor',
  templateUrl: './details-sensor.component.html',
  styleUrls: ['./details-sensor.component.scss'],
})
export class DetailsSensorComponent implements OnInit {
  sensorGroupId = '';
  pageNumber = 1;
  pageSize = 12;
  sensorsData!: ISensorDetails;
  formAddData!: FormGroup;
  showModalAddData = false;
  containsData!: boolean;

  view: [number, number] = [700, 500];
  colorScheme = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA'],
  };

  isUserAdmin = false;
  userId:string | undefined = undefined;

  showXAxis = true;
  showYAxis = true;
  showXAxisLabel = true;
  showYAxisLabel = true;
  xAxisLabel = '';
  yAxisLabel = '';
  userLoggedIn = false;
  // 
  // `Do you want to delete ${sg.name}`
  curlExample = ""

  constructor(
    private activatedRoute: ActivatedRoute,
    private sensorDetailsService: SensorDetailsService,
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private serviceStateStorage:StateStorageService,
    private router: Router
  ) {}

  onSelect(event: any) {
    console.log(event);
  }
  ngOnInit(): void {
    this.activatedRoute.params.subscribe((params) => {
      this.sensorGroupId = params['id'];
      this.getSensorDetailsData(this.sensorGroupId);
    });
    this.formAddData = this.fb.group({
      data: ['', Validators.required],
    });
    this.serviceStateStorage.checkIfJwtSaved()
    this.serviceStateStorage.getAuthData.subscribe(value => {
      this.userLoggedIn = value;
    })

    this.serviceStateStorage.isUserAdmin.subscribe(value => {
      this.isUserAdmin = value
    })

    this.serviceStateStorage.getUserId.subscribe(value => {
      this.userId = value
    })
  }

  toggleShowModalAddData() {
    this.showModalAddData = !this.showModalAddData;
    this.formAddData.reset();
    this.curlExample = `curl --location --request POST 'https://localhost:44388/api/SensorData/Add/fddf28f3-d98c-45d7-b991-0becd2b881e6' --header 'api-key: ${this.sensorsData.sensorHash}' --data 'ADD DATA HERE'`

  }

  onAddData(fb: FormGroup) {
    if (fb.valid) {
      console.log(this.sensorsData.sensorHash);
      this.sensorDetailsService
        .addData(this.sensorGroupId,this.sensorsData.sensorHash, fb.value.data)
        .subscribe((_) => {
          this.getSensorDetailsData(this.sensorGroupId);
          this.showModalAddData = false;
          this.formAddData.reset();
        });
    }
  }

  logout() {
    this.serviceStateStorage.removeJwt();
  }

  navigateToLogin(){
    this.router.navigate(['../auth'])
  }

  private getSensorDetailsData(groupId: string) {
    this.sensorDetailsService
      .getSensorData(groupId, this.pageSize, this.pageNumber)
      .subscribe((data) => {
        this.sensorsData = data;
        this.sensorsData.content[0].series.map((item) => {
          const formattedTimestamp = this.datePipe.transform(
            item.name,
            'HH:mm dd.MMM.yyyy'
          );
          item.name = formattedTimestamp
          return item;
        })
        this.xAxisLabel = this.sensorsData.xAxis;
        this.yAxisLabel = this.sensorsData.yAxis;
        if (this.sensorsData.content[0]?.series.length > 0) {
          this.containsData = true;
        } else {
          this.containsData = false;
        }
      });
  }
}
