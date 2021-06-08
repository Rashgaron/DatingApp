import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

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

  constructor(private accountService: AccountService) {}

  ngOnInit(){
    this.setCurrentUser();
  }

  setCurrentUser(){
    const user: User = JSON.parse(localStorage.getItem("user") || '{}');
    this.accountService.setCurrentUser(user);
  }

}
