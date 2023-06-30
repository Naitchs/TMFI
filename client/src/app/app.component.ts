import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
  title = 'DMNSS';
  users: any;
  
  constructor(private router: Router, private http: HttpClient) { }

  currentRoute: string;
  sideBarOpen = true;

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentRoute = event.url;
      }
    });

    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
       error: error => console.log(error),
       complete: ()=> console.log('Request completed')
    })
   
  }

  sideBarToggler() {
    this.sideBarOpen = !this.sideBarOpen;
  }
}
