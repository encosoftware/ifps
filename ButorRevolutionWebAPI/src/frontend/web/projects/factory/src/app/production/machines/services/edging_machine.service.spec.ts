import { TestBed } from '@angular/core/testing';
import { CncMachineService } from './cnc_machine.service';

describe('CncMachineService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CncMachineService = TestBed.get(CncMachineService);
    expect(service).toBeTruthy();
  });
});
