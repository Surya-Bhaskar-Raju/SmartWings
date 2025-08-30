import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

export interface AddAircraft {
  model: string;
  totalSeats: number;
  economySeats: number;
  businessSeats: number;
}

@Component({
  selector: 'app-aircraft-revenue',
  imports: [FormsModule, CommonModule],
  templateUrl: './aircraft-revenue.component.html',
  styleUrls: ['./aircraft-revenue.component.css'],
})
export class AircraftRevenueComponent implements OnInit {
  aircraftModel: string = '';
  revenue: number | null = null;
  errorMessage: string = '';
  loading: boolean = false;

  // Added for dropdown
  aircraftList: AddAircraft[] = [];
  private getAircraftUrl = 'http://localhost:5152/api/flights/aircrafts';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    // Fetch aircraft list for dropdown
    this.http.get<AddAircraft[]>(this.getAircraftUrl).subscribe(
      (response) => {
        this.aircraftList = response;
      },
      (error) => {
        console.error('Error fetching aircraft list:', error);
      }
    );
  }

  getRevenue() {
    this.errorMessage = '';
    this.revenue = null;

    if (!this.aircraftModel.trim()) {
      this.errorMessage = 'Please enter an aircraft model.';
      return;
    }

    this.loading = true;
    const url = `http://localhost:5152/api/Dashboard/revenue/${this.aircraftModel.trim()}`;

    this.http
      .get<number>(url, { responseType: 'text' as 'json' })
      .subscribe({
        next: (res) => {
          this.revenue = parseFloat(res as unknown as string);
          this.loading = false;
        },
        error: () => {
          this.errorMessage = 'Failed to fetch revenue. Please try again.';
          this.loading = false;
        },
      });
  }
}
