import { TestBed } from '@angular/core/testing';

import { CourseImagesService } from './course-images.service';

describe('CourseImagesService', () => {
  let service: CourseImagesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CourseImagesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
