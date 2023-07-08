import { HttpClient } from '@angular/common/http';
import { Component} from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  users: any;
  
  constructor(private router: Router, private http: HttpClient, public accountService: AccountService) { }

  
}
