import { Component, OnInit } from '@angular/core';
import { CourseImagesService } from 'src/app/services/course-images.service';
import { ApiService } from 'src/app/services/api.service';
import { Router } from '@angular/router';
 
interface Course {
  id: number;
  title: string;       // or name/courseName — match your API!
  description: string; // optional
}
 
@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.css']
})
export class CourseListComponent implements OnInit {
  courses: Course[] = [];
 
  // Make the service public so the template can use it
  constructor(
    private api: ApiService,
    private imageService: CourseImagesService,
    private router: Router
  ) {}
 
  ngOnInit(): void {
    this.api.getAllCourses().subscribe({
      next: (res: Course[]) => {
        console.log("Courses from API:", res);  // 👀 debug
  this.courses = res;
      },
      error: err => console.error("Error fetching courses:", err)
    });
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/home']);
  }

  getCourseImage(title: string): string {
    return this.imageService.getImage(title);
  }
}