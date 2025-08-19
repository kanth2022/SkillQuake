import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { jwtDecode } from 'jwt-decode';
import { CourseImagesService } from 'src/app/services/course-images.service';
import { Router } from '@angular/router';
import { ICourse } from 'src/app/models/course.model';
 
interface Course {
  id: number;
  title: string;
  description: string;
  price: number;
  videoUrl: string;
}
 
@Component({
  selector: 'app-learner-dashboard',
  templateUrl: './learner-dashboard.component.html',
  styleUrls: ['./learner-dashboard.component.css']
})
export class LearnerDashboardComponent implements OnInit {
  courses: ICourse[] = [];
  enrolledCourses: number[] = [];
  enrolledCoursesDetails: Course[] = [];
  learnerId: number | null = null;
 
  ratings: { [courseId: number]: number } = {};
  reviews: { [courseId: number]: string } = {};

 
  constructor(
    private api: ApiService,
    private imageService: CourseImagesService,
    private router: Router
  ) {}
 
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded: any = jwtDecode(token);
      this.learnerId = decoded.nameid || decoded.id || decoded.UserId;
    }
 
    this.loadCourses();
    this.loadEnrollments();
  }
 
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/home']);
  }
 
  loadCourses() {
    this.api.getAllCourses().subscribe({
      next: (res) => {
        // normalize id
        this.courses = res.map((c: any) => ({
          id: c.id || c.courseId,
          title: c.title,
          description: c.description,
          price: c.price,
          videoUrl: c.videoUrl
        }));
      },
      error: (err) => {
        console.error('Error loading courses', err);
      }
    });
  }
 
  loadEnrollments() {
    if (!this.learnerId) return;
 
    this.api.getEnrollmentsByLearner(this.learnerId).subscribe({
      next: (res) => {
        this.enrolledCourses = res.map((c: any) => c.id || c.courseId);
        this.enrolledCoursesDetails = res.map((c: any) => ({
          id: c.id || c.courseId,
          title: c.title,
          description: c.description,
          price: c.price,
          videoUrl: c.videoUrl
        }));
      },
      error: (err) => console.error('Error loading enrollments', err)
    });
  }
 
  getCourseImage(title: string): string {
    return this.imageService.getImage(title);
  }
 
  enroll(courseId: number) {
    if (!this.learnerId) {
      alert('You must be logged in to enroll!');
      return;
    }
 
    const payload = { userId: this.learnerId, courseId: courseId };
 
    this.api.enrollCourse(payload).subscribe({
      next: (res) => {
        alert(res.message || 'Enrolled successfully!');
        this.loadEnrollments();
      },
      error: (err) => {
        console.error('Error enrolling', err);
        alert(err.error || 'Error enrolling in course');
      }
    });
  }
 
  isEnrolled(courseId: number): boolean {
    return this.enrolledCourses.includes(courseId);
  }
 
  openCourse(url: string) {
    if (url) {
      window.open(url, '_blank');
    } else {
      alert('No course content available!');
    }
  }
 
  // ⭐ Rating
  setRating(courseId: number, star: number) {
    this.ratings[courseId] = star;
  }
  
  getRating(courseId: number): number {
    return this.ratings[courseId] || 0;
  }
  
  submitRating(courseId: number) {
    if (!this.learnerId) {
      alert("You must be logged in to rate!");
      return;
    }
  
    const payload = {
      userId: this.learnerId,
      courseId: courseId,
      stars: this.ratings[courseId] || 0,
      review: this.reviews[courseId] || ""
    };
  
    if (payload.stars === 0) {
      alert("Please select a star rating!");
      return;
    }
  
    this.api.addRating(payload).subscribe({
      next: (res: any) => {
        // Handle both object and plain string
        if (typeof res === 'string') {
          alert(res); // plain "Rating submitted successfully."
        } else {
          alert(res.message || 'Rating submitted successfully!');
        }
      },
      error: (err) => {
        console.error("Error submitting rating", err);
        alert(err.error || "Error submitting rating");
      }
    });
  }
  unenroll(courseId: number) {
    if (!this.learnerId) {
      alert("You must be logged in to unenroll!");
      return;
    }
  
    this.api.unenroll(this.learnerId, courseId).subscribe({
      next: (res:any) => {
        alert(res.message || "Unenrolled successfully!");
        this.loadEnrollments(); // refresh list
      },
      error: (err) => {
        console.error("Error unenrolling", err);
        alert(err.error || "Error unenrolling from course");
      }
    });
  }
  

}