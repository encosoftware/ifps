import { TestBed } from '@angular/core/testing';

import { WorkloadService } from './workload.service';

describe('WorkloadService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkloadService = TestBed.get(WorkloadService);
    expect(service).toBeTruthy();
  });
});
