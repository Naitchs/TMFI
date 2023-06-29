import { Component, AfterViewInit } from '@angular/core';
declare const M: any;

@Component({
  selector: 'app-ip-profile',
  templateUrl: './ip-profile.component.html',
  styleUrls: ['./ip-profile.component.scss']
})
export class IpProfileComponent {


  ipUser = {
    profilePicture: null,
    firstName: '',
    lastName: '',
    middleName: '',
    gender: '',
    status: '',
    tribe:'',
    phoneNum: '',
    email:'',
    facebook: '',
    street: '',
    barangay:'',
    city: '',
    churchName:'',
    ministry: '',
    reason: ''
  };

  onSubmit() {
    // Handle form submission logic here
    console.log(this.ipUser);
  }

  onFileSelected(event: Event) {
    const inputElement = event.target as HTMLInputElement;
    if (inputElement.files && inputElement.files[0]) {
      this.ipUser.profilePicture = inputElement.files[0];
    }
  }

  ngAfterViewInit() {
    M.AutoInit();
  }

}
