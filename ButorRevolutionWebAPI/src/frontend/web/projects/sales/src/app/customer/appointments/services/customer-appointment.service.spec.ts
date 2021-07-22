import { TestBed } from '@angular/core/testing';

import { CustomerAppointmentService } from './customer-appointment.service';

describe('CustomerAppointmentService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CustomerAppointmentService = TestBed.get(CustomerAppointmentService);
    expect(service).toBeTruthy();
  });
});
