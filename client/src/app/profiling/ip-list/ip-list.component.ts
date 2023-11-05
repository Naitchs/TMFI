import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Profile } from 'src/app/_models/profile';
import { ProfileService } from 'src/app/_services/profile.service';

@Component({
  selector: 'app-ip-list',
  templateUrl: './ip-list.component.html',
  styleUrls: ['./ip-list.component.scss']
})
export class IpListComponent implements OnInit{

  @Input() profile: Profile | undefined;
  ipProfile: Profile[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;

  constructor(private profileService: ProfileService, private router: Router) { }

  ngOnInit(): void {
    this.profileService.getIps().subscribe(ipProfile => { // Subscribe to the observable here
      this.ipProfile = ipProfile;
    });
  }

  redirectToDetail(publicId: string) {
    this.router.navigate(['/ip-detail', publicId]);
  }
  redirectToEdit(publicId: string) {
    this.router.navigate(['/ip-edit', publicId]);
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
    return this.ipProfile.slice(startIndex, endIndex);
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
    return Math.ceil(this.ipProfile.length / this.itemsPerPage);
  }

}
