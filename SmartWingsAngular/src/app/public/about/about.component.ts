import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent {
  title = 'Smart Wings';
  description = `
    Welcome to Smart Wings – your trusted partner for seamless flight bookings
    and hassle-free travel experiences. We connect you to the world with
    competitive prices, intuitive navigation, and exceptional customer support.
    Whether you’re flying for business or leisure, our platform ensures a smooth
    journey from booking to boarding.
  `;

  moreInfo = `
    Since our inception, Smart Wings has been committed to bringing the best of
    technology and travel together. Our team of passionate travel experts and
    tech innovators work tirelessly to make your booking experience fast, secure,
    and enjoyable. From last-minute trips to planned getaways, we are here to
    make every journey memorable.
  `;

  highlights = [
    { title: '🌍 Global Reach', text: 'Book flights to destinations worldwide with just a few clicks.' },
    { title: '💸 Best Prices', text: 'We guarantee competitive fares with no hidden charges.' },
    { title: '🛡 Secure Booking', text: 'Advanced encryption keeps your payments and data safe.' }
  ];

  teamMembers = [
    { name: 'Sophia Carter', role: 'Founder & CEO', img: 'assets/Sophia Carter.png' },
    { name: 'James Lee', role: 'Head of Operations', img: 'assets/James Lee.jpg' },
    { name: 'Isabella Gomez', role: 'Lead Designer', img: 'assets/Isabella Gomez.jpg' },
    { name: 'Liam Patel', role: 'Chief Technology Officer', img: 'assets/Liam Patel.jpg' }
  ];
}