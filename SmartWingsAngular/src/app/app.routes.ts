import { Routes } from '@angular/router';

import { AboutComponent } from './public/about/about.component';
import { ContactComponent } from './public/contact/contact.component';
import { HomeComponent } from './public/home/home.component';
import { LoginComponent } from './public/login/login.component';

// Import user components
import { UserdashboardComponent } from './user/userdashboard/userdashboard.component';
import { BookFlightComponent } from './user/book-flight/book-flight.component';
import { NotificationsComponent } from './user/notifications/notifications.component';
import { PaymentComponent } from './user/payment/payment.component';
import { SearchFlightsComponent } from './user/search-flights/search-flights.component';
import { ViewpastbookingsComponent } from './user/viewpastbookings/viewpastbookings.component';
import { LandingpageComponent } from './user/landingpage/landingpage.component';
import { SearchbookingbyrefidComponent } from './user/searchbookingbyrefid/searchbookingbyrefid.component';

// Import admin components
import { AddAircraftComponent } from './admin/add-aircraft/add-aircraft.component';
import { AddFlightComponent } from './admin/add-flight/add-flight.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';

import { ViewAircraftComponent } from './admin/view-aircraft/view-aircraft.component';
import { ViewFlightsComponent } from './admin/view-flights/view-flights.component';
import { GetallbookingsComponent } from './admin/getallbookings/getallbookings.component';
import { AircraftRevenueComponent } from './admin/aircraft-revenue/aircraft-revenue.component';
import { UpcomingflightsComponent } from './admin/upcomingflights/upcomingflights.component';

// Import guards
import { authGuard, adminGuard } from './core/guards/auth.guard';
import { EditAircraftComponent } from './admin/edit-aircraft/edit-aircraft.component';
import { RescheduleFlightComponent } from './admin/reschedule-flight/reschedule-flight.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'contact', component: ContactComponent },
  { path: 'login', component: LoginComponent },

  // User dashboard route as layout with children
  {
    path: 'user/dashboard',
    component: UserdashboardComponent,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        component: LandingpageComponent,
      },
      {
        path: 'search-flights',
        component: SearchFlightsComponent,
        canActivate: [authGuard],
      },
      {
        path: 'book-flight/:id',
        component: BookFlightComponent,
        canActivate: [authGuard],
      },
      {
        path: 'viewpastbookings',
        component: ViewpastbookingsComponent,
        canActivate: [authGuard],
      },
       {
        path: 'searchbookingbyrefid',
        component: SearchbookingbyrefidComponent,
        canActivate: [authGuard],
      },
      {
        path: 'payment',
        component: PaymentComponent,
        canActivate: [authGuard],
      },
      {
        path: 'notifications',
        component: NotificationsComponent,
        canActivate: [authGuard],
      },
      {
        path: 'landingpage',
        component: LandingpageComponent,
        canActivate: [authGuard],
      },
      {
        path: '',
        redirectTo: 'search-flights',
        pathMatch: 'full',
      },
    ],
  },

  // Admin routes with children - protected by admin guard
  {
    path: 'admin/dashboard',
    component: DashboardComponent,
    canActivate: [adminGuard],
    children: [
      { path: 'view-flights', component: ViewFlightsComponent },
      { path: 'add-flight', component: AddFlightComponent },
      { path: 'add-aircraft', component: AddAircraftComponent },
      { path: 'view-aircraft', component: ViewAircraftComponent },
      { path: 'edit-aircraft/:id', component: EditAircraftComponent },
      { path: 'reschedule-flight/:id', component: RescheduleFlightComponent },


      { path: 'getallbookings', component: GetallbookingsComponent },
      { path: 'aircraft-revenue', component: AircraftRevenueComponent },
      { path: 'upcomingflights', component: UpcomingflightsComponent },
      { path: '', redirectTo: 'view-flights', pathMatch: 'full' },
    ],
  },

  // Fallback
  { path: 'register', redirectTo: '/login', pathMatch: 'full' }, // Redirect old register route to login
  { path: '**', redirectTo: '', pathMatch: 'full' },
];
