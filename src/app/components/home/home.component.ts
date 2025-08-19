import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit, OnDestroy, AfterViewInit {
 
  categories = [
    { icon: '💃', title: 'Dancing', description: 'Groove to your favorite tunes and learn diverse dance styles.' },
    { icon: '🎤', title: 'Singing', description: 'Hit the high notes with singing enthusiasts.' },
    { icon: '📷', title: 'Photography', description: 'Capture moments and share visual stories.' },
    { icon: '🍳', title: 'Cooking', description: 'Master culinary arts and share delicious creations.' },
    { icon: '🏃', title: 'Sports', description: 'Stay active, connect with athletes, and pursue fitness.' },
    { icon: '🎨', title: 'Arts & Creativity', description: 'Create, express, and join a vibrant artistic community.' }
  ];
   
  images = [
    'assets/sports.jpg',
    'assets/dance.jpg',
    'assets/art.jpg',
    'assets/chess-players.jpg'
  ];
  currentIndex = 0;
  intervalId: any;
 
  constructor(private route: ActivatedRoute) { }
 
  ngOnInit(): void {
    this.route.fragment.subscribe(fragment => {
      if (fragment) {
        const element = document.getElementById(fragment);
        if (element) {
          setTimeout(() => element.scrollIntoView({ behavior: 'smooth',block:'start' }), 0);
        }
      }
    });
    this.startSlideshow();
  }
 
  ngAfterViewInit(): void {
    this.showSlide(this.currentIndex);
  }
 
  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }
 
  startSlideshow(): void {
    this.intervalId = setInterval(() => {
      this.nextSlide();
    }, 3000);
  }
 
  showSlide(index: number): void {
    const slides = document.querySelectorAll('.slider-image');
    if (slides.length === 0) return;
 
    slides.forEach(slide => slide.classList.remove('active'));
    slides[index].classList.add('active');
  }
 
  nextSlide(): void {
    this.currentIndex = (this.currentIndex + 1) % this.images.length;
    this.showSlide(this.currentIndex);
  }
 
  prevSlide(): void {
    this.currentIndex = (this.currentIndex - 1 + this.images.length) % this.images.length;
    this.showSlide(this.currentIndex);
  }
}