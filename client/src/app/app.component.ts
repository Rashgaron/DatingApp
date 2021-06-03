import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

const apiUrl = 'https://localhost:5001/api/users';
//Decorator, way of giving a class extra power

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit{
  title = 'The dating app';
  users:any; // Uses any for any type of return. It can be anything. Whatever it returns

  constructor(private http: HttpClient) {}

  ngOnInit(){
    this.getUsers();
  }




  //funcion to get all users 
  getUsers(){
    this.http.get(apiUrl)
      .subscribe(response => {
        this.users = response;
      }, error => {
        console.log(error);
      });
  }
  
}
