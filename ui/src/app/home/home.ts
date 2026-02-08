
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import usa from '@svg-maps/usa';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrl: './home.css',
  standalone: true
})
export class Home {
  map = usa;

  constructor(private router: Router) { }

  onStateClick(stateId: string) {
    this.router.navigate(['/state', stateId]);
  }
}
