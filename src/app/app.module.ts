import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CourseListComponent } from './course-list/course-list.component';
import { HttpClientModule } from '@angular/common/http';
import { RegisterComponent } from './components/register/register.component'
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from './components/navbar/navbar.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth.interceptor';
import { CoachDashboardComponent } from './components/coach-dashboard/coach-dashboard.component';
import { LearnerDashboardComponent } from './components/learner-dashboard/learner-dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    CourseListComponent,
    RegisterComponent,
    NavbarComponent,
    LoginComponent,
    HomeComponent,
    CoachDashboardComponent,
    LearnerDashboardComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
