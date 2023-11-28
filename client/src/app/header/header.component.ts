import { Component, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../_services/account.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
  @Output() toggleSidebar: EventEmitter<any> = new EventEmitter();
  isWideScreen: boolean = true;

  constructor(private router: Router, public accountService: AccountService, private route: ActivatedRoute,) {}

  ngOnInit(): void {}

  onToggleSidebar() {
    this.toggleSidebar.emit();
    this.checkScreenSize();
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

  @HostListener('window:resize', ['$event'])
  checkScreenSize(event?: Event) {
    const screenWidth = window.innerWidth;
    this.isWideScreen = screenWidth > 600; 
  }

}