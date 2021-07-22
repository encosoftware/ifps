import { TestBed } from '@angular/core/testing';

import { MeetingroomsService } from './meetingrooms.service';

describe('MeetingroomsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MeetingroomsService = TestBed.get(MeetingroomsService);
    expect(service).toBeTruthy();
  });
});
