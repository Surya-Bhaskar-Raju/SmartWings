import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { isPlatformBrowser, CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, CommonModule, HttpClientModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  private isBrowser: boolean;
  private logoutUrl = 'http://localhost:5152/api/auth/logout';

  constructor(
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object,
    private http: HttpClient
  ) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  get showDashboardUser(): boolean {
    if (this.isBrowser) {
      const token = sessionStorage.getItem('token');
      const userString = sessionStorage.getItem('user');

      if (token && userString) {
        const user = JSON.parse(userString);
        return user.role === 'User'; // case-sensitive
      }
    }
    return false;
  }
    get showDashboardAdmin(): boolean {
    if (this.isBrowser) {
      const token = sessionStorage.getItem('token');
      const userString = sessionStorage.getItem('user');

      if (token && userString) {
        const user = JSON.parse(userString);
        return user.role === 'Admin'; // case-sensitive
      }
    }
    return false;
  }

  get isLoggedIn(): boolean {
    if (this.isBrowser) {
      const token = sessionStorage.getItem('token');
      return !!token;
    }
    return false;
  }

  get isOnLoginPage(): boolean {
    return this.router.url === '/login';
  }

  logout() {
    if (!this.isBrowser) {
      return;
    }

    const token = sessionStorage.getItem('token');

    if (token) {
      // Call backend logout endpoint to revoke token
      this.http.post(this.logoutUrl, {}, { responseType: 'text' }).subscribe({
        next: (response) => {
          // Backend logout successful
        },
        error: (error) => {
          // Continue with local logout even if backend fails
        },
        complete: () => {
          // Clear local storage regardless of backend response
          this.clearLocalSession();
        },
      });
    } else {
      // No token, just clear local storage
      this.clearLocalSession();
    }
  }

  // Backup logout method for testing
  logoutImmediate() {
    this.clearLocalSession();
  }

  private clearLocalSession() {
    if (this.isBrowser) {
      sessionStorage.clear();
      localStorage.clear(); // Clear localStorage as well for complete cleanup
    }
    this.router.navigate(['/login']);
  }
}
