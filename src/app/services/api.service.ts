import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseUrl = 'https://localhost:7065/api';

  constructor(private http: HttpClient) { }
  getCourses(): Observable<any[]>
   {
    return this.http.get<any[]>('${this.baseUrl}/courses');
   }
   register (data:any) : Observable<any> {
    return this.http.post<any>('https://localhost:7065/api/Auth/register',data);
   }
   login(data: any) {
    return this.http.post('https://localhost:7065/api/auth/login', data);
    }
  createCourse(courseData:any){
    return this.http.post('https://localhost:7065/api/Courses',courseData);
  }
   // Get all courses created by a specific coach
   getCoursesByCoach(coachId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Courses/coach/${coachId}`);
  }
 
  // Delete a course by ID
  deleteCourse(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Courses/${id}`);
  }

  getAllCourses() {
    return this.http.get<any[]>(`${this.baseUrl}/courses`);
  }
 
  // Enroll learner into a course
enrollCourse(payload: any) {
  return this.http.post<any>(`${this.baseUrl}/Enrollment/enroll`, payload, {
    headers: { 'Content-Type': 'application/json' }
  });
}
 
// Get learner’s enrollments
getEnrollmentsByLearner(userId: number) {
  return this.http.get<any[]>(`${this.baseUrl}/Enrollment/user/${userId}`);
}

// Add Rating
addRating(payload: any): Observable<any> {
  return this.http.post(`${this.baseUrl}/Ratings`, payload ,{ responseType: 'text'});
  }

  unenroll(userId: number, courseId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Enrollment/unenroll`, {
      body: { userId, courseId }, // backend expects userId & courseId
    });
  }

}


