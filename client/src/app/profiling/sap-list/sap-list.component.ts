import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Sap } from 'src/app/_models/sap';
import { ProfileService } from 'src/app/_services/profile.service';
declare var $: any;

@Component({
  selector: 'app-sap-list',
  templateUrl: './sap-list.component.html',
  styleUrls: ['./sap-list.component.scss']
})
export class SapListComponent {

  @Input() profile: Sap | undefined;
  sapProfile: Sap [] = [];
  sapId: number;

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;

  successMessage: string | null = null;
  errorMessage: string | null = null;
  errorMessageModal: string | null = null;


  constructor(private profileService: ProfileService, private router: Router) { }


  ngOnInit(): void {
    this.profileService.sapRegistered$.subscribe(sap => {
      this.sapProfile.push(sap);
    });
    this.profileService.getSaps().subscribe(sapProfile => { // Subscribe to the observable here
      // this.ipProfile = ipProfile;
      this.sapProfile = Object.values(sapProfile);
    });

    this.sapId = 0;

  }

  redirectToDetail(publicId: string) {
    this.router.navigate(['/sap-detail', publicId]);
  }

  redirectToEdit(publicId: string) {
    this.router.navigate(['/sap-edit', publicId]);
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

  modalHide() {
    $('#proceedModals').modal('hide');
  }

  deleteClick(id: number) {
    this.sapId = id;
  }

  deleteSap() {
    $('#proceedModals').modal('hide');

    const index = this.sapProfile.findIndex(c => c.id === this.sapId);
    console.log("Index ng tinanggal na course:", index);

    if (this.sapId == 0) return;

    this.profileService.deleteSap(this.sapId).subscribe(
      sap => {
        this.successMessage = 'Deleted Successfully!';
        this.sapId = 0;

        if (index !== -1) {
          this.sapProfile.splice(index, 1);
        }
      }, (error) => {
        this.errorMessage = 'Failed to delete';
        this.sapId = 0;
      }
    )
  }

}
