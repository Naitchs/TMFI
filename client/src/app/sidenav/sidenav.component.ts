import { Component, Input, OnInit } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],

  animations: [
    trigger('expandAnimation', [
      state('collapsed', style({ height: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition('collapsed <=> expanded', animate('200ms ease-in-out')),
    ]),
  ],
})
export class SidenavComponent implements OnInit{

  @Input() sideBarOpen: boolean = true;


  constructor(private router: Router, public accountService: AccountService) {}

  ngOnInit() {
    // Component initialization code...
  }



}
