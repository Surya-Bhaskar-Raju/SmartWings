import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css'],
})
export class PaymentComponent implements OnInit {
  amount: number = 0;
  paymentMethod: string = '';
  upiId: string = '';
  cardNumber: string = '';
  cvv: string = '';
  bookingId: string = '';
  userId: string = '';
  showSuccessModal: boolean = false;
  isProcessing: boolean = false;

  private paymentUrl = 'http://localhost:5152/api/payment/process';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    const bookingJson = sessionStorage.getItem('pendingBooking');
    const userJson = sessionStorage.getItem('user');

    if (!bookingJson || !userJson) {
      alert('Missing booking or user data.');
      this.router.navigate(['/user/search-flights']);
      return;
    }

    const booking = JSON.parse(bookingJson);
    const user = JSON.parse(userJson);

    this.bookingId = booking.bookingId;
    this.amount = booking.totalAmount;
    this.userId = user.userId || user.id; // ✅ handles both cases
  }

  processPayment() {
    // Validate payment method
    if (!this.paymentMethod) {
      alert('❌ Please select a payment method.');
      return;
    }

    // Validate UPI
    if (this.paymentMethod === 'UPI') {
      if (!this.upiId || !this.upiId.includes('@')) {
        alert('❌ Please enter a valid UPI ID.');
        return;
      }
    }

    // Validate Card
    if (this.paymentMethod === 'Credit Card') {
      if (
        !this.cardNumber ||
        this.cardNumber.length !== 16 ||
        !this.cvv ||
        this.cvv.length !== 3
      ) {
        alert('❌ Please enter valid card details.');
        return;
      }
    }

    // ✅ Payload matches backend PaymentRequestDto exactly
    const payload = {
      bookingId: this.bookingId,
      userId: this.userId,
      upiId: this.paymentMethod === 'UPI' ? this.upiId : null,
      cardNumber: this.paymentMethod === 'Credit Card' ? this.cardNumber : null,
      cvv: this.paymentMethod === 'Credit Card' ? this.cvv : null,
      paymentMethod: this.paymentMethod,
      amount: this.amount,
    };

    this.isProcessing = true;
    this.http.post(this.paymentUrl, payload).subscribe({
      next: (res: any) => {
        this.isProcessing = false;
        sessionStorage.removeItem('pendingBooking');
        this.bookingId = res.bookingId || this.bookingId;
        this.showSuccessModal = true;

        // ⏳ Auto-close modal and navigate after 2 seconds
        setTimeout(() => {
          this.showSuccessModal = false;
          this.router.navigate(['/user/dashboard/viewpastbookings']);
        }, 2000);
      },
      error: (err) => {
        this.isProcessing = false;
        alert(err.error?.message || '❌ Payment failed. Please try again.');
      },
    });
  }

  closeModal() {
    this.showSuccessModal = false;
    this.router.navigate(['/user/dashboard']);
  }
}
