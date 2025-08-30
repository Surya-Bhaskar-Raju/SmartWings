import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-landingpage',
  imports: [CommonModule,FormsModule],
  templateUrl: './landingpage.component.html',
  styleUrl: './landingpage.component.css'
})
export class LandingpageComponent {

  constructor(private router: Router) { }

  onBtnClick(){
    this.router.navigate(['/user/dashboard/search-flights']);
  }
}
