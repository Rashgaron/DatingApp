import { Component } from '@angular/core';

//Decorator, way of giving a class extra power

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'The dating app';
}
