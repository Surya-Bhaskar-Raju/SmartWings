import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-searchbookingbyrefid',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './searchbookingbyrefid.component.html',
  styleUrl: './searchbookingbyrefid.component.css'
})
export class SearchbookingbyrefidComponent {
referenceId: string = '';
  booking: any = null;
  errorMessage: string = '';
  loading: boolean = false;

  private bookingUrl = 'http://localhost:5152/api/Booking/track';

  constructor(private http: HttpClient, private router: Router) {}

  searchBooking(): void {
    if (!this.referenceId.trim()) {
      this.errorMessage = '❌ Please enter a valid booking reference.';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.booking = null;

    this.http.get(`${this.bookingUrl}/${this.referenceId}`).subscribe({
      next: (res: any) => {
        this.booking = res;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.errorMessage = err.error?.message || '❌ Booking not found or invalid reference.';
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/user/home']);
  }
}
