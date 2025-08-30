import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import SearchFlight from './search-flight';

@Injectable({
  providedIn: 'root'
})
export class SearchFlightService {
  private apiUrl = 'http://localhost:5152/api/flights';

  constructor(private http: HttpClient) { }

  getFlights(origin: string, destination: string, departureTime: Date | null) :Observable<SearchFlight[]>{
    let params = new HttpParams();

    if (origin) {
      params = params.set('origin', origin);
    }
    if (destination) {
      params = params.set('destination', destination);
    }
    if (departureTime) {
      const dateObj = new Date(departureTime); // Ensure it's a Date
      if (!isNaN(dateObj.getTime())) {         // Check for valid date
        params = params.set('departureDate', dateObj.toISOString());
  }
    }

    return this.http.get<SearchFlight[]>(this.apiUrl, { params });
  }

  getFlightById(id: string): Observable<SearchFlight> {
    return this.http.get<SearchFlight>(`${this.apiUrl}/${id}`);
  }

  updateFlight(flight: SearchFlight): Observable<SearchFlight> {
    return this.http.put<SearchFlight>(`${this.apiUrl}/${flight.flightId}`, flight);
  }
}

