import { inject, Inject, PLATFORM_ID } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { isPlatformBrowser } from '@angular/common';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const platformId = inject(PLATFORM_ID);

  if (isPlatformBrowser(platformId)) {
    const token = sessionStorage.getItem('token');

    if (token) {
      return true;
    } else {
      router.navigate(['/login']);
      return false;
    }
  } else {
    // Server side - deny or allow (usually deny)
    return false;
  }
};

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const platformId = inject(PLATFORM_ID);

  if (isPlatformBrowser(platformId)) {
    const token = sessionStorage.getItem('token');
    const user = sessionStorage.getItem('user');

    if (token && user) {
      const userData = JSON.parse(user);
      if (userData.role === 'Admin') {
        return true;
      }
    }

    router.navigate(['/login']);
    return false;
  } else {
    return false; // server side
  }
};
