import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Profile } from 'src/app/_models/profile';
import { ProfileService } from 'src/app/_services/profile.service';

@Component({
  selector: 'app-ip-detail',
  templateUrl: './ip-detail.component.html',
  styleUrls: ['./ip-detail.component.scss']
})
export class IpDetailComponent implements OnInit{

  ipProfile: Profile | undefined;
  id: number | undefined;

  constructor(private profileService: ProfileService, private route: ActivatedRoute) {}

 
  ngOnInit(): void{
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      if (this.id) {
        this.loadIp(this.id);
      }
    });
  }


  loadIp(id: number) {
    this.profileService.getIp(id).subscribe({
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
  
  
  


  // loadIp(){
  //   const id = this.route.snapshot.paramMap.get('id');
  //   if (!id) return;
  
  //   // Convert the id from string to number
  //   const idNumber = Number(id);
  
  //   if (isNaN(idNumber)) {
  //     console.error('Invalid ID:', id);
  //     return;
  //   }
  
  //   this.profileService.getIp(idNumber).subscribe({
  //     next: ipProfile => {
  //       this.ipProfile = ipProfile;
  //     }
  //   });
  // }
  
}
