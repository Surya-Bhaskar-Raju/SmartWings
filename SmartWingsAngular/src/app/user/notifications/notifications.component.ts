import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

interface Notification {
  id: string;
  userId: string;
  bookingId: string;
  message: string;
  createdAt: string;
  isRead: boolean;
}

@Component({
  selector: 'app-notifications',
  imports: [FormsModule, CommonModule],
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css'],
})
export class NotificationsComponent implements OnInit {
  notifications: Notification[] = [];
  userId: string | null = null;
  errorMsg: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const userJson = sessionStorage.getItem('user');
    const user = userJson ? JSON.parse(userJson) : null;
    const userId = user?.userId;
    this.userId = userId;
    if (this.userId) {
      this.fetchNotifications(this.userId);
    } else {
      this.errorMsg = 'User not logged in';
    }
  }

  fetchNotifications(userId: string) {
    const url = `http://localhost:5152/api/Notification/user/${userId}`;
    this.http.get<Notification[]>(url).subscribe({
      next: (data) => {
        this.notifications = data;
      },
      error: (err) => {
        this.errorMsg = 'Failed to load notifications';
        console.error(err);
      },
    });
  }
}
