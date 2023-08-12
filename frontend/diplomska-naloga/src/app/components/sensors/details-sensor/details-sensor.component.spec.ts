import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsSensorComponent } from './details-sensor.component';

describe('DetailsSensorComponent', () => {
  let component: DetailsSensorComponent;
  let fixture: ComponentFixture<DetailsSensorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetailsSensorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailsSensorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
