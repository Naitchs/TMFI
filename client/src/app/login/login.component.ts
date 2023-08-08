import { Component, OnDestroy, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

import { NavigationEnd, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject, filter, takeUntil } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy{

  model: any = {};
  private ngUnsubscribe = new Subject<void>();
  
  constructor(
    public accountService: AccountService, 
    private router: Router, 
    private toastr: ToastrService
    ) { }

  ngOnInit(): void {
    this.router.events
    .pipe(
      filter((event) => event instanceof NavigationEnd),
      takeUntil(this.ngUnsubscribe)
    )
    .subscribe(() => {
      // Close the modal when navigating away from the login page
      const modal = document.getElementById('loginModal');
      if (modal) {
        modal.classList.remove('show');
      }
    });
  }


  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: () => this.router.navigateByUrl('/members'),
      
      // error: error => this.toastr.error(error.error)
    })
  }

  // logout(){
  //   this.accountService.logout();
  //   this.router.navigateByUrl('/');
  // }

  
}
