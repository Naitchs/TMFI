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
        console.log(ipProfile);
        this.ipProfile = ipProfile;
      }
    });
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
