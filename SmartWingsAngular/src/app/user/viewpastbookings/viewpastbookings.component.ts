import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-viewpastbookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './viewpastbookings.component.html',
  styleUrl: './viewpastbookings.component.css'
})
export class ViewpastbookingsComponent implements OnInit {
  bookings: any[] = [];   // Stores fetched bookings for the user
  userId: string = '';    // Current logged-in user ID
  loading: boolean = false; // Loader flag to show/hide loading state

  private bookingUrl = 'http://localhost:5152/api/Booking'; // API base URL

  constructor(private http: HttpClient, private router: Router) {}

  /**
   * Lifecycle hook: called once when component initializes
   * - Retrieves user from sessionStorage
   * - Extracts UserId
   * - Fetches bookings for the user
   */
  ngOnInit(): void {
    const userJson = sessionStorage.getItem('user');
    if (!userJson) {
      console.error('User not found in sessionStorage.');
      return;
    }

    const user = JSON.parse(userJson);
    this.userId = user.userId || user.id; // ✅ Handles cases where backend returns userId or id

    if (!this.userId) {
      console.error('User ID is missing - cannot fetch bookings.');
      return;
    }

    this.fetchBookings();
  }

  /**
   * Fetches all bookings for the logged-in user
   */
  fetchBookings(): void {
    this.loading = true; // Show loader
    this.http.get<any[]>(`${this.bookingUrl}/user/${this.userId}/bookings`)
      .subscribe({
        next: (data) => {
          this.bookings = data; // Assign API response
          this.loading = false; // Hide loader
        },
        error: (err) => {
          console.error('Error fetching bookings:', err);
          this.loading = false;
        }
      });
  }

  /**
   * Checks if a booking is eligible for cancellation
   * - Only confirmed bookings
   * - Must be at least 8 hours before departure
   */
  canCancel(booking: any): boolean {
    if (booking.status !== 'Confirmed') return false;

    const now = new Date();
    const departure = new Date(booking.departureTime);
    const timeDiffInHours = (departure.getTime() - now.getTime()) / (1000 * 60 * 60);

    return departure > now && timeDiffInHours >= 8; // ✅ business rule
  }

  /**
   * Cancels the booking by referenceId
   * @param referenceId - Booking Reference
   */
  cancelBooking(referenceId: string): void {
    const confirmed = confirm('Are you sure you want to cancel this booking?');
    if (!confirmed) return;

    this.http.put(`${this.bookingUrl}/cancel/${referenceId}`, null)
      .subscribe({
        next: () => {
          // Update local booking list
          const booking = this.bookings.find(b => b.bookingReference === referenceId);
          if (booking) {
            booking.status = 'Cancelled';
          }
          alert('Booking cancelled successfully.');
        },
        error: (err) => {
          console.error('Cancellation failed:', err);
          // alert('Failed to cancel booking. Please try again.');
        }
      });
  }

  /**
   * Returns display-friendly status
   * - If booking is in future → show backend status
   * - If departure time has passed:
   *    - Confirmed → "Visited"
   *    - Cancelled → "Not Visited"
   */
  getDisplayStatus(booking: any): string {
    const now = new Date();
    const departure = new Date(booking.departureTime);

    if (departure > now) {
      return booking.status; // 'Confirmed' or 'Cancelled'
    }

    return booking.status === 'Confirmed' ? 'Visited' : 'Not Visited';
  }
}
