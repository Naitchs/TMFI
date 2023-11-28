// import { HttpClient } from '@angular/common/http';
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
  
  constructor(private router: Router, public accountService: AccountService) { }
  // private http: HttpClient,

  currentRoute: string;
  sideBarOpen = false;
  contentMargin = 0;

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });

    // this.getUsers();
    this.setCurrentUser();
  }


  sideBarToggler() {
    this.sideBarOpen = !this.sideBarOpen;
    this.adjustContentMargin();
  }

  adjustContentMargin() {
    this.contentMargin = this.sideBarOpen ? 100 : 0;
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
}
