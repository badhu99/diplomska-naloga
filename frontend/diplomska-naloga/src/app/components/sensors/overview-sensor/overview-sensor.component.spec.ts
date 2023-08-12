import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OverviewSensorComponent } from './overview-sensor.component';

describe('OverviewSensorComponent', () => {
  let component: OverviewSensorComponent;
  let fixture: ComponentFixture<OverviewSensorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OverviewSensorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OverviewSensorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
