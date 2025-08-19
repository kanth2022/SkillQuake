import {  NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CourseListComponent } from './course-list/course-list.component';  
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/home/home.component';
import { AuthGuard } from './auth.guard';
import { CoachDashboardComponent } from './components/coach-dashboard/coach-dashboard.component';
import { LearnerDashboardComponent } from './components/learner-dashboard/learner-dashboard.component';


const routes: Routes = [
  {path: '',redirectTo:'home',pathMatch: 'full' },
  {path: 'home',component:HomeComponent},
    {path:'courses', component: CourseListComponent},
    {path:'register',component:RegisterComponent},
    {path:'login',component: LoginComponent},
    {path:'home',component: HomeComponent},
    {path:'coach-dashboard',component:CoachDashboardComponent,canActivate:[AuthGuard]},
    {path:'learner-dashboard',component:LearnerDashboardComponent,canActivate:[AuthGuard]}
      
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {anchorScrolling: 'enabled',scrollOffset:[0,80]})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
