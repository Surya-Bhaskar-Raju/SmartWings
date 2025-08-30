import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-book-flight',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './book-flight.component.html',
  styleUrls: ['./book-flight.component.css']
})
export class BookFlightComponent implements OnInit {
  flightId!: string; // Store flight ID from route
  flight: any; // Store flight details
  passengers = [{ fullName: '', age: 0, gender: '', passportNumber: '' }]; // Default passenger form
  selectedClass = ''; // Travel class (Economy/Business)
  classOptions = ['Economy', 'Business']; // Dropdown options
  private flightUrl = 'http://localhost:5152/api/flights'; // API for flight details
  private bookingUrl = 'http://localhost:5152/api/Booking/book'; // API for booking

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    // ✅ Get flight ID from URL and load flight details
    this.flightId = this.route.snapshot.paramMap.get('id')!;
    this.loadFlightDetails();
  }

  // ✅ API call to get flight details
  loadFlightDetails() {
    this.http.get(`${this.flightUrl}/${this.flightId}`).subscribe({
      next: res => this.flight = res,
      error: err => alert('Failed to load flight details')
    });
  }

  // ✅ Add a new passenger form (max 6)
  addPassenger() {
    if (this.passengers.length >= 6) {
      alert('Maximum 6 passengers allowed per booking.');
      return;
    }
    this.passengers.push({ fullName: '', age: 0, gender: '', passportNumber: '' });
  }

  // ✅ Remove passenger form by index
  deletePassenger(index: number) {
    this.passengers.splice(index, 1);
  }

  // ✅ Handle booking and redirect to payment
  proceedToPayment() {
    // Get logged-in user from session storage
    const userJson = sessionStorage.getItem('user');
    const user = userJson ? JSON.parse(userJson) : null;
    const userId = user?.userId;

    // Check if user is logged in
    if (!userId) {
      alert('Please log in first.');
      this.router.navigate(['/login']);
      return;
    }

    // Validate travel class
    if (!this.selectedClass) {
      alert('Please select a travel class.');
      return;
    }

    // Validate passengers
    if (!this.passengers || this.passengers.length === 0) {
      alert('Please add at least one passenger before proceeding.');
      return;
    }

    // Check if any passenger fields are incomplete
    const hasIncompletePassenger = this.passengers.some(p =>
      !p.fullName || !p.age || !p.gender || !p.passportNumber
    );

    if (hasIncompletePassenger) {
      alert('Please fill in all passenger details.');
      return;
    }

    // ✅ Prepare booking request payload
    const bookingPayload = {
      userId: userId,
      flightId: this.flightId,
      class: this.selectedClass,
      passengers: this.passengers
    };

    // ✅ Send booking request to backend
    this.http.post(this.bookingUrl, bookingPayload).subscribe({
      next: (res: any) => {
        // Save booking details in session for payment step
        sessionStorage.setItem('pendingBooking', JSON.stringify({
          bookingId: res.bookingId,
          totalAmount: res.totalAmount,
          passengers: res.passengers,
          flightId: res.flightId
        }));
        console.log(bookingPayload);
        // Navigate to payment page
        this.router.navigate(['/user/dashboard/payment']);
      },
      // Uncomment for error handling
      // error: err => {
      //   alert('❌ Booking failed. Please try again.');
      // }
    });
  }

}
