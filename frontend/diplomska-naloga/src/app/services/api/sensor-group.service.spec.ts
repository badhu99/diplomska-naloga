import { TestBed } from '@angular/core/testing';

import { SensorGroupService } from './sensor-group.service';

describe('SensorGroupService', () => {
  let service: SensorGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SensorGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
