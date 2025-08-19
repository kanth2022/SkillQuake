import { registerLocaleData } from '@angular/common';
import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service'; 
import { Router } from '@angular/router'; 
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username = '';
  email = '';
  phone = '';
  password = '';
  confirmPassword = '';
  role='learner';
 
  constructor(private api: ApiService, private router: Router) {}
 
  register() {
    if (this.password !== this.confirmPassword) {
      alert("Passwords do not match");
      return;
    }
 
    const payload = {
      name: this.username,
      email: this.email,
      phone: this.phone,
      passwordHash: this.password,
      confirmPassword:this.confirmPassword,
      role: this.role,
       // or allow selecting role later
    };
 
    this.api.register(payload).subscribe({
      next: () => {
        alert('Registered successfully');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Registration error:', err); // Shows full error in console
     
        if (err.error?.message) {
          alert('Registration failed: ' + err.error.message);
        } else if (err.error?.title) {
          alert('Registration failed: ' + err.error.title);
        } else if (err.status === 0) {
          alert('Registration failed: Cannot reach server. Is backend running?');
        } else {
          alert('Registration failed with status: ' + err.status);
        }
      }
    })}};