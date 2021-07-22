import { TestBed } from '@angular/core/testing';

import { DecorboardService } from './decorboard.service';

describe('DecorboardService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DecorboardService = TestBed.get(DecorboardService);
    expect(service).toBeTruthy();
  });
});
