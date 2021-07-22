import { TestBed } from '@angular/core/testing';
import { MaterialPackageService } from './material-packages.service';

describe('MaterialPackageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MaterialPackageService = TestBed.get(MaterialPackageService);
    expect(service).toBeTruthy();
  });
});
