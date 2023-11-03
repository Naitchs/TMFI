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

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;


  constructor(private profileService: ProfileService, private router: Router) { }


  ngOnInit(): void {
    this.profileService.getSaps().subscribe(sapProfile => { // Subscribe to the observable here
      this.sapProfile = sapProfile;
    });
  }

  redirectToDetail(publicId: string) {
    this.router.navigate(['/sap-detail', publicId]);
  }

  caps(str: string): string {
    if (!str) return str;
  
    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }
  
  get paginatedList() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.sapProfile.slice(startIndex, endIndex);
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
  
  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }
  
  get totalPages() {
    return Math.ceil(this.sapProfile.length / this.itemsPerPage);
  }

}
