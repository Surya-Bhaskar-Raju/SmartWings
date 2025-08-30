import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-userdashboard',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './userdashboard.component.html',
  styleUrls: ['./userdashboard.component.css'],  // fixed here
})
export class UserdashboardComponent implements OnInit {
  userId: string | null = null;
  userName: string | null = null;

  ngOnInit(): void {
    const userJson = sessionStorage.getItem('user');
    const user = userJson ? JSON.parse(userJson) : null;

    this.userId = user?.userId || null;
    this.userName = user?.userName || 'Guest';
  }
}
