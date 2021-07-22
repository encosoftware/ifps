import { TestBed } from '@angular/core/testing';

import { FoilService } from './foil.service';

describe('FoilService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FoilService = TestBed.get(FoilService);
    expect(service).toBeTruthy();
  });
});
