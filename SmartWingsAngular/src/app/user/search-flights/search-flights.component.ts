import { CommonModule, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import SearchFlight from '../../search-flight';
import { SearchFlightService } from '../../search-flight.service';

@Component({
  selector: 'app-search-flights',
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './search-flights.component.html',
  styleUrl: './search-flights.component.css',
})
export class SearchFlightsComponent {
  origin: string = '';
  destination: string = '';
  departureTime: Date | null = null;
  // List of available flights
  flights: SearchFlight[] = [];
  isLoading = false;
  errMsg = '';
  // List of cities for flight search
  cities = [
    "Agartala","Agatti","Ahmedabad","Aizawl","Akola","Allahabad","Amritsar","Aurangabad","Ayodhya",
    "Bagdogra","Bengaluru","Bhopal","Bhubaneswar","Chandigarh","Chennai","Coimbatore","Daman","Darbhanga",
    "Dehradun","Delhi","Dibrugarh","Dimapur","Goa","Guwahati","Hyderabad","Imphal","Indore","Itanagar",
    "Jaipur","Jammu","Jamnagar","Jeypore","Jodhpur","Kannur","Kanpur","Kochi","Kolkata","Kozhikode",
    "Lucknow","Madurai","Mangaluru","Mumbai","Nagpur","Nashik","Pune","Rajkot","Ranchi","Shillong",
    "Shimla","Siliguri","Srinagar","Surat","Thiruvananthapuram","Tiruchirappalli","Tirupati","Udaipur",
    "Vadodara","Varanasi","Vijayawada","Visakhapatnam"
  ];
  
  constructor(
    private searchFlightService: SearchFlightService,
    private router: Router
  ) {}

  // Searches for flights based on user input
  searchFlights() {
    this.isLoading = true;
    this.errMsg = '';
    // Search for flights using the service
    this.searchFlightService
      .getFlights(this.origin, this.destination, this.departureTime)
      .subscribe({
        next: (flights) => {
          const now = new Date();

          flights.forEach(f => {
            // Mark as expired if arrival is in the past
            if (new Date(f.arrivalTime) < now && f.status !== 'Expired') {
              f.status = 'Expired';
              this.searchFlightService.updateFlight(f).subscribe({
                error: (err) => console.error('Error updating expired flight:', err)
              });
            }
          });

          // Keep only available flights (not Cancelled or Expired)
          this.flights = flights.filter(f => f.status !== 'Cancelled' && f.status !== 'Expired');

          this.isLoading = false;
        },
        // Handle errors
        error: () => {
          this.errMsg = 'Error fetching flights';
          this.isLoading = false;
        },
      });
  }

  /**
   * Books a flight by navigating to the booking page
   * param flightId - ID of the flight to book
   */
  bookFlight(flightId: string) {
    this.router.navigate(['/user/dashboard/book-flight', flightId]);
  }
}
