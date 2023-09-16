import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Sap } from 'src/app/_models/sap';
import { ProfileService } from 'src/app/_services/profile.service';

@Component({
  selector: 'app-sap-list',
  templateUrl: './sap-list.component.html',
  styleUrls: ['./sap-list.component.scss']
})
export class SapListComponent {

  @Input() profile: Sap | undefined;
  sapProfile: Sap [] = [];

  constructor(private profileService: ProfileService, private router: Router) { }


  ngOnInit(): void {
    this.profileService.getSaps().subscribe(sapProfile => { // Subscribe to the observable here
      this.sapProfile = sapProfile;
    });
  }

  redirectToDetail(id: number) {
    this.router.navigate(['/sap-detail', id]);
  }

  caps(str: string): string {
    if (!str) return str;
  
    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

}
