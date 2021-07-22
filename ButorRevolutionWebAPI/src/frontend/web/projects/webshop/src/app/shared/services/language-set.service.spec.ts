import { TestBed } from '@angular/core/testing';

import { LanguageSetService } from './language-set.service';

describe('LanguageSetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LanguageSetService = TestBed.get(LanguageSetService);
    expect(service).toBeTruthy();
  });
});
