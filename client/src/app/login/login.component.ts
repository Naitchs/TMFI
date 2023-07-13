import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

import { User } from '../_models/user';
import { Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit{

  model: any = {};
  
  constructor(public accountService: AccountService, private router: Router, 
    private toastr: ToastrService){}

  ngOnInit(): void {
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
