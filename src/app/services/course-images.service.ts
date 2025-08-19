import { Injectable } from '@angular/core';
 
@Injectable({
  providedIn: 'root'
})
export class CourseImagesService {
 
  private courseImages: { [key: string]: string } = {
    'Dance Course': 'assets/images/dancing.jpg',
    'Photography': 'assets/images/Photography.jpg',
    'Cooking': 'assets/images/cooking.jpg',
    'Cricket': 'assets/images/Cricket.jpg',
    'Art Course': 'assets/images/Art Course.jpg',
    'Classical Dancing': 'assets/images/Classical.jpg',
    'Singing': 'assets/images/Singing.jpg',
    'Default': 'assets/images/default.jpg' 
  };
 
  constructor() {}

  getImage(title: string): string {
    const normalizedTitle = title.trim();
    return this.courseImages[normalizedTitle] || this.courseImages['Default'];
  }
  
}
 