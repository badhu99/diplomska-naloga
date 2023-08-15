import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SensorGroup } from 'src/app/interfaces/sensor-group';
import { SensorGroupService } from 'src/app/services/api/sensor-group.service';
import { StateStorageService } from 'src/app/services/common/state-storage.service';

@Component({
  selector: 'app-overview-sensor',
  templateUrl: './overview-sensor.component.html',
  styleUrls: ['./overview-sensor.component.scss'],
})
export class OverviewSensorComponent implements OnInit {
  pageNumber = 1;
  pageSize = 12;
  orderDesc = false;
  orderBy = 0;

  showModal = false;
  newGroupForm!: FormGroup;

  sensorGroups: SensorGroup[] = [];

  constructor(
    private serviceSensorGroup: SensorGroupService,
    private router: Router,
    private fb: FormBuilder,
    private serviceStateStorage : StateStorageService
  ) {}
  ngOnInit(): void {
    this.getPaginatedSensorGroup();
    this.newGroupForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(4)]],
      xAxis: ['', Validators.required],
      yAxis: ['', Validators.required],
    });
  }

  onCreateNew(fb: FormGroup) {
    if (fb.valid) {
      this.serviceSensorGroup.createNew(fb.value.name, fb.value.xAxis, fb.value.yAxis).subscribe((id) => {
        this.showModal = false;
        fb.reset()
        this.router.navigate(['sensors', id])
      });
    }
  }

  openSensorGroup(sg: SensorGroup) {
    console.log('Navigate to sensorGroupID: ', sg.id);
    this.router.navigate(['sensors', sg.id]);
  }

  toggleModalCreateNew() {
    this.showModal = !this.showModal;
    this.newGroupForm.reset();
  }

  deleteSensorGroup(sg: SensorGroup) {
    const deleteGroup = window.confirm(`Do you want to delete ${sg.name}`);
    if (deleteGroup) {
      this.serviceSensorGroup.deleteSensorGroup(sg.id).subscribe((_) => {
        this.sensorGroups = this.sensorGroups.filter((s) => s.id !== sg.id);
      });
    }
  }

  logout() {
    this.serviceStateStorage.removeJwt();
  }

  private getPaginatedSensorGroup() {
    this.serviceSensorGroup
      .getSensorGroupsPagination(
        this.pageNumber,
        this.pageSize,
        this.orderDesc,
        this.orderBy
      )
      .subscribe(
        (data) => {
          console.log(data);
          this.sensorGroups = data.items;
        },
        (err) => console.error(err)
      );
  }
}
