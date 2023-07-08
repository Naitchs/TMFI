import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'DMNSS';
  users: any;
  
  constructor(private router: Router, private http: HttpClient, public accountService: AccountService) { }

  currentRoute: string;
  sideBarOpen = true;

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });

    this.getUsers();
    this.setCurrentUser();
  }

  sideBarToggler() {
    this.sideBarOpen = !this.sideBarOpen;
  }

  getUsers(){
        this.http.get('https://localhost:5001/api/users').subscribe({
          next: response => this.users = response,
          error: error => console.log(error),
          complete: ()=> console.log('Request completed')
    })
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
