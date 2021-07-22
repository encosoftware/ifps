import { TestBed } from '@angular/core/testing';

import { FurnitureUnitsService } from './furniture-units.service';

describe('FurnitureUnitsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FurnitureUnitsService = TestBed.get(FurnitureUnitsService);
    expect(service).toBeTruthy();
  });
});
