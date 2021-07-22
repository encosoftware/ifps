import { TestBed } from '@angular/core/testing';
import { WFUService } from './wfurnitureunits.service';

describe('WFUService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WFUService = TestBed.get(WFUService);
    expect(service).toBeTruthy();
  });
});
