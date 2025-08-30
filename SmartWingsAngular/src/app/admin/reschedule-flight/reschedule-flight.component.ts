import { Component } from '@angular/core';
import SearchFlight from '../../search-flight';
import { ActivatedRoute, Router } from '@angular/router';
import { SearchFlightService } from '../../search-flight.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-reschedule-flight',
  imports: [FormsModule,CommonModule],
  templateUrl: './reschedule-flight.component.html',
  styleUrl: './reschedule-flight.component.css'
})
export class RescheduleFlightComponent {
  // Holds the flight data to be edited
  flight: SearchFlight = {
    flightId: '',
    flightNumber: '',
    origin: '',
    destination: '',
    departureTime: new Date(),
    arrivalTime: new Date(),
    airCraftId: '',
    status: '',
    priceEconomy: 0,
    priceBusiness: 0
  }

  // List of available cities for selection
  cities = [
      "Agartala","Agatti","Ahmedabad","Aizawl","Akola","Allahabad","Amritsar","Aurangabad","Ayodhya",
      "Bagdogra","Bengaluru","Bhopal","Bhubaneswar","Chandigarh","Chennai","Coimbatore","Daman","Darbhanga",
      "Dehradun","Delhi","Dibrugarh","Dimapur","Goa","Guwahati","Hyderabad","Imphal","Indore","Itanagar",
      "Jaipur","Jammu","Jamnagar","Jeypore","Jodhpur","Kannur","Kanpur","Kochi","Kolkata","Kozhikode",
      "Lucknow","Madurai","Mangaluru","Mumbai","Nagpur","Nashik","Pune","Rajkot","Ranchi","Shillong",
      "Shimla","Siliguri","Srinagar","Surat","Thiruvananthapuram","Tiruchirappalli","Tirupati","Udaipur",
      "Vadodara","Varanasi","Vijayawada","Visakhapatnam"
    ];


  // Error and success message handling
  errorMessage: string | null = null;
  successMessage: string | null = null;
  isUpdated: boolean = false;

  constructor(
    private route: ActivatedRoute, // For accessing route parameters
    private searchFlightService: SearchFlightService, // Service for flight API calls
    private router: Router // For navigation after update
  ) {}

  // Lifecycle hook: runs when component is initialized
  ngOnInit() {
    // Get flight ID from route if present
    const flightId = this.route.snapshot.paramMap.get('id');
    if (flightId) {
      // Fetch flight details by ID
      this.searchFlightService.getFlightById(flightId).subscribe({
        next: (flight: SearchFlight) => {
          // Ensure city names match exactly from the list
          this.flight = { ...flight, flightId: flight.flightId };
          if (!this.cities.includes(this.flight.origin.trim())) {
             const match = this.cities.find(city => city.toLowerCase() === this.flight.origin.toLowerCase());
             if (match) this.flight.origin = match;
          }},
        error: (err: any) => {
          console.error('Error fetching flight:', err);
        }
      });
    }
  }

  // Updates the flight details using the service
  updateFlight(flight: SearchFlight) {
    this.searchFlightService.updateFlight(flight).subscribe({
      next: () => {
        this.successMessage = 'Flight updated successfully!';
        this.isUpdated = true;
        this.router.navigate(['/admin/dashboard/view-flights']);
      },
      error: (err: any) => {
        console.error('Error updating flight:', err);
        this.errorMessage = 'Failed to update flight.';
      }
    });
  }
}
