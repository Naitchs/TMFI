import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Profile } from 'src/app/_models/profile';
import { ProfileService } from 'src/app/_services/profile.service';
declare var $: any;

@Component({
  selector: 'app-ip-list',
  templateUrl: './ip-list.component.html',
  styleUrls: ['./ip-list.component.scss']
})
export class IpListComponent implements OnInit{

  @Input() profile: Profile | undefined;
  ipProfile: Profile[] = [];
  ipId: number;

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;

  successMessage: string | null = null;
  errorMessage: string | null = null;
  errorMessageModal: string | null = null;

  constructor(private profileService: ProfileService, private router: Router) {

   }

  ngOnInit(): void {
    this.profileService.ipRegistered$.subscribe(ip => {
      this.ipProfile.push(ip);
    });
    this.profileService.getIps().subscribe(ipProfile => { // Subscribe to the observable here
      // this.ipProfile = ipProfile;
      this.ipProfile = Object.values(ipProfile);
    });

    this.ipId = 0;
  }

  redirectToDetail(publicId: string) {
    this.router.navigate(['/ip-detail', publicId]);
  }
  redirectToEdit(publicId: string) {
    this.router.navigate(['/ip-edit', publicId]);
  }

  deleteClick(id: number) {
    this.ipId = id;
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

  modalHide() {
    $('#proceedModals').modal('hide');
  }


  deleteIp() {
    $('#proceedModals').modal('hide');

    const index = this.ipProfile.findIndex(c => c.id === this.ipId);
    console.log("Index ng tinanggal na course:", index);

    if (this.ipId == 0) return;

    this.profileService.deleteIp(this.ipId).subscribe(
      ip => {
        this.successMessage = 'Deleted Successfully!';
        this.ipId = 0;

        if (index !== -1) {
          this.ipProfile.splice(index, 1);
        }
      }, (error) => {
        this.errorMessage = 'Failed to delete';
        this.ipId = 0;
      }
    )
  }


}
