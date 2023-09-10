import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  @Output() toggleSidebar: EventEmitter<any> = new EventEmitter();

  constructor(private router: Router, public accountService: AccountService) {}

  ngOnInit(): void {}

  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}