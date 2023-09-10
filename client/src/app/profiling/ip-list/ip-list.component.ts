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

  constructor(private profileService: ProfileService, private router: Router) { }

  ngOnInit(): void {
    this.profileService.getIps().subscribe(ipProfile => { // Subscribe to the observable here
      this.ipProfile = ipProfile;
    });
  }

  redirectToDetail(id: number) {
    this.router.navigate(['/ip-detail', id]);
  }

}
