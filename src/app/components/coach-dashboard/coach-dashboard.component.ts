import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
 
@Component({
  selector: 'app-coach-dashboard',
  templateUrl: './coach-dashboard.component.html',
  styleUrls: ['./coach-dashboard.component.css']
})
export class CoachDashboardComponent implements OnInit {
  title = '';
  description = '';
  price: number | null = null;
  videoUrl = '';
 
  coachId: number | null = null;
  myCourses: any[] = [];
 
  constructor(private api: ApiService, private router: Router) {}
 
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      const decoded: any = jwtDecode(token);
this.coachId = decoded.nameid || decoded.UserId || decoded.id || decoded.userId;
      if (this.coachId) {
        this.getMyCourses();
      }
    }
  }
 
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/home']);
  }
 
  createCourse() {
    const token = localStorage.getItem('token');
    if (!token) return;
 
    const decoded: any = jwtDecode(token);
const coachIdFromToken = decoded.nameid || decoded.UserId || decoded.id || decoded.userId;
 
    if (!coachIdFromToken) {
      alert('Unable to find Coach ID in token.');
      return;
    }
 
    const payload = {
      coachId: coachIdFromToken,
      title: this.title,
      description: this.description,
      videoUrl: this.videoUrl,
      price: this.price
    };
 
    this.api.createCourse(payload).subscribe({
      next: () => {
        alert('Course created successfully!');
        this.title = '';
        this.description = '';
        this.price = null;
        this.videoUrl = '';
        this.getMyCourses(); // refresh list
      },
      error: (error) => {
        console.error('Create Course Error:', error);
        alert(error?.error?.message || 'Error creating course.');
      }
    });
  }
 
  getMyCourses() {
    if (!this.coachId) return;
 
    this.api.getCoursesByCoach(this.coachId).subscribe({
      next: (res) => {
        this.myCourses = res;
      },
      error: () => {
        this.myCourses = [];
      }
    });
  }
 
  deleteCourse(id: number) {
    if (!confirm('Are you sure you want to delete this course?')) return;
 
    this.api.deleteCourse(id).subscribe({
      next: () => {
        alert('Course deleted successfully!');
this.myCourses = this.myCourses.filter(c => c.id !== id);
      },
      error: (err) => {
        alert(err?.error?.message || 'Error deleting course.');
      }
    });
  }
}