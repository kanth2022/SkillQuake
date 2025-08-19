import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearnerDashboardComponent } from './learner-dashboard.component';

describe('LearnerDashboardComponent', () => {
  let component: LearnerDashboardComponent;
  let fixture: ComponentFixture<LearnerDashboardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LearnerDashboardComponent]
    });
    fixture = TestBed.createComponent(LearnerDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
