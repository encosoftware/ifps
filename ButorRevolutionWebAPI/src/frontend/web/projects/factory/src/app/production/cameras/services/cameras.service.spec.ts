import { TestBed } from '@angular/core/testing';
import { CameraService } from './cameras.service';

describe('CameraService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CameraService = TestBed.get(CameraService);
    expect(service).toBeTruthy();
  });
});
