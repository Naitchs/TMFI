import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Profile } from 'src/app/_models/profile';
import { ProfileService } from 'src/app/_services/profile.service';

@Component({
  selector: 'app-ip-detail',
  templateUrl: './ip-detail.component.html',
  styleUrls: ['./ip-detail.component.scss']
})
export class IpDetailComponent implements OnInit{

  ipProfile: Profile | undefined;
  publicId: string | undefined;

  constructor(private profileService: ProfileService, private route: ActivatedRoute,
    private router: Router) {}

 
  ngOnInit(): void{
    this.route.params.subscribe(params => {
      this.publicId = params['publicId'];
      if (this.publicId) {
        this.loadIp(this.publicId);
      }
    });
  }

  redirectToEdit(publicId: string) {
    this.router.navigate(['/ip-edit', publicId]);
  }

  loadIp(publicId: string) {
    this.profileService.getIp(publicId).subscribe({
      next: ipProfile => {
        // console.log(ipProfile);
        this.ipProfile = ipProfile;
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
