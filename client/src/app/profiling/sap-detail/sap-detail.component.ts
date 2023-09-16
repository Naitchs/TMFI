import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Sap } from 'src/app/_models/sap';
import { ProfileService } from 'src/app/_services/profile.service';

@Component({
  selector: 'app-sap-detail',
  templateUrl: './sap-detail.component.html',
  styleUrls: ['./sap-detail.component.scss']
})
export class SapDetailComponent {


  sapProfile: Sap | undefined;
  id: number | undefined;

  constructor(private profileService: ProfileService, private route: ActivatedRoute) {}

 
  ngOnInit(): void{
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      if (this.id) {
        this.loadSap(this.id);
      }
    });
  }


  loadSap(id: number) {
    this.profileService.getSap(id).subscribe({
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
