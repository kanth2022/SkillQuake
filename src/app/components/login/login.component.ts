import { Component } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { Router } from '@angular/router';
import {jwtDecode} from 'jwt-decode';

interface LoginResponse{
  id:number;
  name:string;
  email:string;
  role:string;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {
  email = '';
  password = '';
 
  constructor(private api: ApiService, private router:Router) {}
 
  login() {
    const payload = {
      email: this.email,
      passwordHash: this.password
    };
 
    this.api.login(payload).subscribe({
      next: (res: any) => {
        localStorage.setItem('token',res.token);

        const decoded: any = jwtDecode(res.token);

        // alert(`Welcome, ${res.name}!`);

        if(decoded.role === 'Coach'){
          this.router.navigate(['/coach-dashboard']);
          alert('Welcome Coach!');
        }else{
          this.router.navigate(['/learner-dashboard']);
          alert('Welcome Learner!');
        }
      },
      error: (err) => {
        console.error('Login failed:', err);
        alert('Invalid email or password');
      }
    });
  }
}
 