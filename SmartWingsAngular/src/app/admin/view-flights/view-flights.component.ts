import { Component } from '@angular/core';
import SearchFlight from '../../search-flight';
import { SearchFlightService } from '../../search-flight.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-view-flights',
  imports: [FormsModule, CommonModule],
  templateUrl: './view-flights.component.html',
  styleUrl: './view-flights.component.css'
})
export class ViewFlightsComponent {
  // Holds the list of flights fetched from the API
  flights: SearchFlight[] = [];
  // Indicates if data is being loaded
  isLoading = false;
  // Stores error message if fetching fails
  errMsg = '';

  // Optional filters for searching flights
  origin = "";
  destination = "";
  departureTime: Date | null = null;

  constructor(
    private searchFlightService: SearchFlightService,
    private router: Router
  ) {}

  // Lifecycle hook: runs when component is initialized
  ngOnInit() {
    this.loadFlights();
  }

  // Loads flights from the backend API, marks expired, and sorts them
  loadFlights() {
    this.isLoading = true;
    this.errMsg = '';

    this.searchFlightService.getFlights(this.origin, this.destination, this.departureTime)
      .subscribe({
        next: (flights) => {
          const now = new Date();

          flights.forEach(f => {
            // Mark expired flights
            if (new Date(f.arrivalTime) < now && f.status !== 'Expired') {
              f.status = 'Expired';
              this.searchFlightService.updateFlight(f).subscribe({
                error: (err) => console.error('Error updating expired flight:', err)
              });
            }
          });

          // Sort: Scheduled → Delayed → Cancelled → Expired
          const order: { [key: string]: number } = {
            'Scheduled': 1,
            'Delayed': 2,
            'Cancelled': 3,
            'Expired': 4
          };

          this.flights = flights.sort((a, b) => order[a.status] - order[b.status]);

          this.isLoading = false;
        },
        error: () => {
          this.errMsg = 'Error fetching flights';
          this.isLoading = false;
        }
      });
  }

  // Navigates to the reschedule page for the selected flight
  rescheduleFlight(flight: SearchFlight) {
    this.router.navigate(['/admin/dashboard/reschedule-flight', flight.flightId]);
  }

  // Cancels the selected flight after confirmation
  cancelFlight(flight: SearchFlight) {
    if (confirm("Are you sure you want to cancel this flight?")) {
      this.searchFlightService.updateFlight({ ...flight, status: 'Cancelled' }).subscribe({
        next: () => this.loadFlights(),
        error: (err) => console.error('Error cancelling flight:', err)
      });
    }
  }
}
