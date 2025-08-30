import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent {
  contactInfo = [
    { icon: '📍', title: 'Address', text: '123 Aviation Street, SkyCity, USA' },
    { icon: '📞', title: 'Phone', text: '+1 234 567 890' },
    { icon: '✉', title: 'Email', text: 'support@smartwings.com' }
  ];

  formData = {
    name: '',
    email: '',
    message: ''
  };

  onSubmit() {
    console.log('Form submitted:', this.formData);
    alert('Thank you for contacting Smart Wings! We will get back to you soon.');
    this.formData = { name: '', email: '', message: '' };
  }
}