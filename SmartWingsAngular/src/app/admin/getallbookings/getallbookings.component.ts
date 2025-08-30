import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

interface Passenger {
  passengerId: string;
  fullName: string;
  age: number;
  gender: string;
  passportNumber: string;
  seatNumber: string;
  seatClass: string;
}

interface Booking {
  bookingId: string;
  bookingReferenceId: string;
  flightId: string;
  flightNumber: string;
  origin: string;
  destination: string;
  departureTime: string;
  bookingDate: string;
  userId: string;
  userName: string;
  email: string;
  price: number;
  status: string;
  passengers: Passenger[];
}
interface ViewAircraft {
  model: string;
}
interface Getflightsforeachmodel {
  flightNumber: string;
}

@Component({
  selector: 'app-getallbookings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './getallbookings.component.html',
  styleUrls: ['./getallbookings.component.css'],
})
export class GetallbookingsComponent implements OnInit {
  bookings: Booking[] = [];
  aircrafts: ViewAircraft[] = [];
  flights: Getflightsforeachmodel[] = [];
  flightNumber: string = '';
  departureDate?: string;
  aircraftModel: string = '';
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadAircrafts();
    //this.loadflights();
  }

  getAllBookings(): void {
    if (!this.flightNumber.trim()) {
      this.bookings = [];
      return;
    }
    //query parameter ->aircraftmodel and departure date
    //path parameter ->flightNumber
    let url = `http://localhost:5152/api/Dashboard/bookings/${this.flightNumber.trim()}`;
    url += `?aircraftModel=${this.aircraftModel}`;
    //if date is mentioned ,then use date in url...else ignore date filtering
    if (this.departureDate) {
      url += `&departureDate=${this.departureDate}`;
    }

    this.http.get<Booking[]>(url).subscribe({
      next: (data) => (this.bookings = data),
      error: (err) => {
        console.error('Error fetching bookings', err);
        this.bookings = [];
      },
    });
  }
  // Loads all aircrafts from the backend API
  private loadAircrafts() {
    this.http
      .get<ViewAircraft[]>('http://localhost:5152/api/flights/aircrafts')
      .subscribe(
        (data: ViewAircraft[]) => {
          this.aircrafts = data;
        },
        (error: any) => {
          console.error('Error fetching aircrafts:', error);
        }
      );
  }
  private loadflights() {
    this.http
      .get<Getflightsforeachmodel[]>(
        `http://localhost:5152/api/Dashboard/flight-with-model/${this.aircraftModel}`
      )
      .subscribe(
        (data: Getflightsforeachmodel[]) => {
          this.flights = data;
        },
        (error: any) => {
          console.error('Error fetching flights:', error);
        }
      );
  }
  onAircraftChange() {
    this.loadflights();
    this.flightNumber = ''; // reset selected flight
  }

  exportToPDF(): void {
    if (this.bookings.length === 0) return;

    // 📌 Create PDF in Landscape mode
    const doc = new jsPDF({
      orientation: 'landscape',
      unit: 'pt',
      format: 'a4',
    });

    // Title
    doc.setFontSize(16);
    doc.text(
      `Bookings Report for Flight ${this.flightNumber} (Model: ${this.aircraftModel})`,
      40,
      40
    );

    // Table for Bookings (now includes Reference ID + Status + Model)
    const bookingTableData = this.bookings.map((b) => [
      b.bookingReferenceId, // ✅ use Reference ID instead of bookingId
      b.flightNumber,
      `${b.origin}  ${b.destination}`,
      new Date(b.departureTime).toLocaleString(),
      b.userName,
      b.email,
      `${b.price}`,
      b.status,
      this.aircraftModel,
    ]);

    autoTable(doc, {
      head: [
        [
          'Reference ID',
          'Flight No',
          'Route',
          'Departure',
          'User',
          'Email',
          'Price',
          'Status',
          'Aircraft Model',
        ],
      ],
      body: bookingTableData,
      startY: 60,
      styles: {
        fontSize: 10,
        cellPadding: 4,
        lineColor: [0, 0, 0],
        lineWidth: 0.2,
      },
      headStyles: {
        halign: 'center',
        lineColor: [0, 0, 0],
        lineWidth: 0.2,
      },
      margin: { left: 40, right: 40 },
    });

    // For each booking, also add passenger details on a new landscape page
    this.bookings.forEach((booking) => {
      doc.addPage('a4', 'landscape');
      doc.setFontSize(14);
      doc.text(`Reference ID: ${booking.bookingReferenceId}`, 40, 40); // ✅ changed
      doc.text(`Booked By: ${booking.userName} (${booking.email})`, 40, 60);
      doc.text(`Route: ${booking.origin} ${booking.destination}`, 40, 80);
      doc.text(
        `Departure: ${new Date(booking.departureTime).toLocaleString()}`,
        40,
        100
      );
      doc.text(`Price: ${booking.price}`, 40, 120);
      doc.text(`Status: ${booking.status}`, 40, 140);
      doc.text(`Aircraft Model: ${this.aircraftModel}`, 40, 160);

      if (booking.passengers.length > 0) {
        const passengerTable = booking.passengers.map((p) => [
          p.fullName,
          p.gender,
          p.age,
          p.seatNumber,
          p.seatClass,
          p.passportNumber,
        ]);

        autoTable(doc, {
          head: [['Name', 'Gender', 'Age', 'Seat', 'Class', 'Passport']],
          body: passengerTable,
          startY: 180,
          styles: {
            fontSize: 10,
            cellPadding: 4,
            lineColor: [0, 0, 0],
            lineWidth: 0.2,
          },
          headStyles: {
            halign: 'center',
            lineColor: [0, 0, 0],
            lineWidth: 0.2,
          },
          margin: { left: 40, right: 40 },
        });
      }
    });

    doc.save(`Bookings_${this.flightNumber}.pdf`);
  }
}
