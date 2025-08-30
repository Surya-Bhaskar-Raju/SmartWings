import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { ViewAircraft } from '../view-aircraft/view-aircraft';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-aircraft',
  templateUrl: './edit-aircraft.component.html',
  styleUrls: ['./edit-aircraft.component.css'],
  imports:[FormsModule,CommonModule]
})
export class EditAircraftComponent implements OnInit {
  // Holds aircraft data for binding to the form
  aircraft: ViewAircraft = {
    airCraftId: '',
    model: '',
    totalSeats: 0,
    economySeats: 0,
    businessSeats: 0
  };

  // Determines if the component is in edit mode or add mode
  isEditMode = false;

  constructor(
    private route: ActivatedRoute, // For accessing route parameters
    private http: HttpClient,      // For making HTTP requests
    private router: Router         // For navigation after save
  ) {}

  // Lifecycle hook: runs when component is initialized
  ngOnInit() {
    // Get aircraft ID from route if present (edit mode)
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.loadAircraft(id);
    }
  }

  // Loads aircraft data from API for editing
  loadAircraft(id: string) {
    this.http.get<ViewAircraft>(`http://localhost:5152/api/flights/aircrafts/${id}`).subscribe(
      (data) => {
        this.aircraft = data;
      },
      (error) => {
        console.error('Error loading aircraft:', error);
      }
    );
  }

  // Saves the aircraft (PUT request if editing)
  saveAircraft() {
    if (this.isEditMode) {
      this.http.put(`http://localhost:5152/api/flights/aircrafts/${this.aircraft.airCraftId}`, this.aircraft)
        .subscribe(() => {
          // Navigate back to aircraft list after saving
          this.router.navigate(['/admin/dashboard/view-aircraft']);
        });
    }
  }
}
