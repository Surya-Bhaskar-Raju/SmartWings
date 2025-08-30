import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ViewAircraft } from './view-aircraft';
import { HttpClient } from '@angular/common/http';
import { Router,RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-view-aircraft',
  imports: [FormsModule,CommonModule],
  templateUrl: './view-aircraft.component.html',
  styleUrl: './view-aircraft.component.css'
})
export class ViewAircraftComponent {
  // Holds the list of aircrafts fetched from the API
  aircrafts: ViewAircraft[] = [];
  // Stores the aircraft to be deleted (for confirmation and alert)
  aircraftToBeDeleted: ViewAircraft | null = null;

  constructor(private http: HttpClient,private router: Router,) {}

  // Lifecycle hook: runs when component is initialized
  ngOnInit(){
    this.loadAircrafts();
  }

  // Loads all aircrafts from the backend API
  private loadAircrafts() {
    this.http.get<ViewAircraft[]>('http://localhost:5152/api/flights/aircrafts').subscribe(
      (data: ViewAircraft[]) => {
        this.aircrafts = data;
      },
      (error: any) => {
        console.error('Error fetching aircrafts:', error);
      }
    );
  }

  // Loads a single aircraft by ID (used before deletion)
  private loadAirCraftById(airCraftId: string) {
    this.http.get<ViewAircraft>(`http://localhost:5152/api/flights/aircrafts/${airCraftId}`).subscribe(
      (data: ViewAircraft) => {
        // Handle the aircraft data
        this.aircraftToBeDeleted = data;

      },
      (error: any) => {
        console.error('Error fetching aircraft:', error);
      }
    );
  }

  // Navigates to the edit page for the selected aircraft
  editAircraft(airCraftId: string) {
    // Implement edit functionality
    console.log(airCraftId);
    this.router.navigate(['/admin/dashboard/edit-aircraft', airCraftId]);
  }

  // Deletes the selected aircraft after confirmation
  deleteAircraft(airCraftId: string) {
    this.loadAirCraftById(airCraftId);
    if (confirm("Are you sure you want to delete this aircraft?")) {
      this.http.delete(`http://localhost:5152/api/flights/aircrafts/${airCraftId}`).subscribe(
        () => {
          this.aircrafts = this.aircrafts.filter(a => a.airCraftId !== airCraftId);
          alert(`Aircraft ${this.aircraftToBeDeleted?.model} deleted successfully.`);
        },
        (error: any) => {
          console.error('Error deleting aircraft:', error);
        }
      );
    }
  }

}
