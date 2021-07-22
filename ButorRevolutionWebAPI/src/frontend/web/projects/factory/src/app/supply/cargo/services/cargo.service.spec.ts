import { TestBed } from '@angular/core/testing';

import { CargoService } from './cargo.service';

describe('CargoService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CargoService = TestBed.get(CargoService);
    expect(service).toBeTruthy();
  });
});
