import { TestBed } from '@angular/core/testing';

import { PackingService } from './packing.service';

describe('PackingService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PackingService = TestBed.get(PackingService);
    expect(service).toBeTruthy();
  });
});
