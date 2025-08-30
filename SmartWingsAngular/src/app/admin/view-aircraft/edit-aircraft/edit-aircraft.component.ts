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
  aircraft: ViewAircraft = {
    airCraftId: '',
    model: '',
    totalSeats: 0,
    economySeats: 0,
    businessSeats: 0
  };
  isEditMode = false;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.loadAircraft(id);
    }
  }

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

  saveAircraft() {
    if (this.isEditMode) {
      this.http.put(`http://localhost:5152/api/flights/aircrafts/${this.aircraft.airCraftId}`, this.aircraft)
        .subscribe(() => {
          this.router.navigate(['/admin/dashboard/view-aircraft']);
        });
    }
  }
}
