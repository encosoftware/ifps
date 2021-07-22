import { TestBed } from '@angular/core/testing';

import { WorkTopService } from './work-top.service';

describe('WorkTopService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkTopService = TestBed.get(WorkTopService);
    expect(service).toBeTruthy();
  });
});
