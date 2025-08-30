import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
    title = 'Welcome to SmartWings';
  subtitle = 'Your journey starts here – book flights with ease, speed, and security.';
  ctaText = 'Book Now';

  destinations = [
    { name: 'Paris', image: 'assets/paris.jpg' },
    { name: 'Tokyo', image: 'assets/Tokyo.jpg' },
    { name: 'New York', image: 'assets/NewYork.jpg' },
    { name: 'Dubai', image: 'assets/Dubai.jpg' },
    { name: 'Sydney', image: 'assets/Sydney.jpeg' },
  ];

  testimonials = [
    { name: 'Alice', feedback: 'Amazing booking experience! Everything was smooth and quick.' },
    { name: 'Bob', feedback: 'Super fast and easy. Saved me so much time.' },
    { name: 'Charlie', feedback: 'Loved the customer service! They helped me even at 2 AM.' },
    { name: 'Diana', feedback: 'The best flight booking platform I’ve ever used.' },
  ];

  constructor(private router: Router) {}

  onCTAClick() {
    const token = sessionStorage.getItem('token');
    if (token) {
      this.router.navigate(['/user/dashboard/search-flights']);
    } else {
      this.router.navigate(['/login']);
    }
  }
}