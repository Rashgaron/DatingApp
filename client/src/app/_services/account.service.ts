import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from "rxjs/operators";
import { User } from '../_models/user';
//Angular services are singletons

@Injectable({
  providedIn: 'root'
})

export class AccountService {

  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) =>{
        const user = response;
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user:User) =>{
        if(user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }


  setCurrentUser(user: any){

    user
    ? this.currentUserSource.next(user)
    : this.resetCurrentUser();

  }

  logout(){
    localStorage.removeItem('user');
    this.resetCurrentUser();
  }

  resetCurrentUser(){
    this.currentUserSource = new ReplaySubject<User>(1);
    this.currentUser$ = this.currentUserSource.asObservable();
  }
}
