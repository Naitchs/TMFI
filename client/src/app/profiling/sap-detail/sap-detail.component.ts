import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Sap } from 'src/app/_models/sap';
import { ProfileService } from 'src/app/_services/profile.service';

@Component({
  selector: 'app-sap-detail',
  templateUrl: './sap-detail.component.html',
  styleUrls: ['./sap-detail.component.scss']
})
export class SapDetailComponent {


  sapProfile: Sap | undefined;
  publicId: string | undefined;

  constructor(private profileService: ProfileService, private route: ActivatedRoute,
    private router: Router) {}

 
  ngOnInit(): void{
    this.route.params.subscribe(params => {
      this.publicId = params['publicId'];
      if (this.publicId) {
        this.loadSap(this.publicId);
      }
    });
  }

  redirectToDetail(publicId: string) {
    this.router.navigate(['/sap-edit', publicId]);
  } 
  
  loadSap(publicId) {
    this.profileService.getSap(publicId).subscribe({
      next: sapProfile => {
        // console.log(ipProfile);
        this.sapProfile = sapProfile;
      }
    });
  }

  caps(str: string): string {
    if (!str) return str;
  
    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }
  
  
  

  getFormattedDate(dateOfBirth: string): string {
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateOfBirth).toLocaleDateString(undefined, options);
  }
  

}
