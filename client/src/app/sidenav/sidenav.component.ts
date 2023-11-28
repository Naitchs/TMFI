import { ChangeDetectorRef, Component, ElementRef, EventEmitter, HostListener, Input, OnChanges, OnInit, Output, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Router } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { Member } from '../_models/member';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],

  animations: [
    trigger('slideAnimation', [
      state('collapsed', style({ transform: 'translateX(-280px)', visibility: 'hidden' })),
      state('expanded', style({ transform: 'translateX(0)', visibility: 'visible' })),
      transition('collapsed <=> expanded', animate('200ms ease-in-out')),
    ]),
  ],
})
export class SidenavComponent implements OnInit, OnChanges {

  @ViewChild('sidebar') sidebar: ElementRef;
  @Input() sideBarOpen: boolean = false;
  public animationState: string;
  @Output() sidebarStateChanged: EventEmitter<boolean> = new EventEmitter<boolean>();


  constructor(
    private router: Router,
    public accountService: AccountService,
    private cdr: ChangeDetectorRef,
    private renderer: Renderer2,
    private el: ElementRef
  ) {}

  ngOnInit() {
    this.setAnimationState();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.sideBarOpen) {
      this.setAnimationState();
      this.sidebarStateChanged.emit(this.sideBarOpen);
    }
  }

  private setAnimationState() {
    this.animationState = this.sideBarOpen ? 'expanded' : 'collapsed';
  }

  // ngAfterViewInit() {
  //   this.renderer.listen('document', 'click', (event: Event) => {
  //     const isToggleBtnClicked = (event.target as HTMLElement).classList.contains('navbar-toggler');

  //     // Check if the clicked element is outside the sidebar and the sidebar is open
  //     if (!this.sidebar.nativeElement.contains(event.target as HTMLElement) && !isToggleBtnClicked && this.sideBarOpen) {
  //       this.sideBarOpen = false;
  //       this.cdr.detectChanges();
  //     }
  //   });
  // }

  @HostListener('document:click', ['$event'])
  closeSidebarOnClickOutside(event: MouseEvent) {
    const isToggleBtnClicked = (event.target as HTMLElement).classList.contains('navbar-toggler');

    if (!this.sidebar.nativeElement.contains(event.target as HTMLElement) && !isToggleBtnClicked && this.sideBarOpen) {
      this.sideBarOpen = false;
      this.sidebarStateChanged.emit(this.sideBarOpen);
    }
  }
  

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
  


}
