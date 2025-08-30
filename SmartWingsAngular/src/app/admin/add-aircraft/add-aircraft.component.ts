import { Component } from '@angular/core';
import { AddAircraft } from './add-aircraft';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-aircraft',
  imports: [FormsModule,CommonModule],
  templateUrl: './add-aircraft.component.html',
  styleUrl: './add-aircraft.component.css',
  standalone: true
})
export class AddAircraftComponent {
  aircraft: AddAircraft = {
    model: '',
    totalSeats: 0,
    economySeats: 0,
    businessSeats: 0
  };
  isAdded: boolean = false;
  // API endpoint for adding aircraft
  private apiUrl = 'http://localhost:5152/api/flights/aircrafts';

  constructor(private http: HttpClient) {}
  // Adds a new aircraft
  addAircraft() {
    this.http.post(this.apiUrl, this.aircraft).subscribe(response => {
      console.log('Aircraft added:', response);
    });
    this.isAdded = true;
    this.aircraft = {
      model: '',
      totalSeats: 0,
      economySeats: 0,
      businessSeats: 0
    };
  }


  }

