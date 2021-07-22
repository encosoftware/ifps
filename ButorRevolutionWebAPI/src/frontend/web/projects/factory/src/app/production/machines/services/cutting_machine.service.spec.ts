import { TestBed } from '@angular/core/testing';

import { CuttingMachineService } from './cutting_machine.service';

describe('CuttingMachineService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CuttingMachineService = TestBed.get(CuttingMachineService);
    expect(service).toBeTruthy();
  });
});
