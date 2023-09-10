import { Component, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ProfileService } from 'src/app/_services/profile.service';
declare const M: any;

@Component({
  selector: 'app-ip-profile',
  templateUrl: './ip-profile.component.html',
  styleUrls: ['./ip-profile.component.scss']
})
export class IpProfileComponent {

  ipForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string [] | undefined;

  constructor(private profileService: ProfileService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router){ }

  

  private getDateOnly(dob: string | undefined){
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
    .toISOString().slice(0,10);
  }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initializeForm(){
    this.ipForm = this.fb.group({
      firstname: ['', Validators.required],
      middlename: [''],
      lastname: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required],
      status: ['', Validators.required],
      tribe: ['', Validators.required],
      phonenum: [''],
      email: [''],
      facebook: [''],
      street: ['', Validators.required],
      barangay: ['', Validators.required],
      city: ['', Validators.required],
      churchname: ['', Validators.required],
      ministry: [''],
      reason: [''],
    });
  }

  registerIp(){
    const dob = this.getDateOnly(this.ipForm.controls['dateOfBirth'].value);
    const values = {...this.ipForm.value, dateOfBirth: dob};
    this.profileService.registerProfile(values).subscribe(
      () => {
        this.toastr.success('Profile registered successfully', 'Success');
        this.ipForm.reset(); // This will reset all the form fields
      },
      (error) => {
        this.toastr.error('Error registering profile', 'Error');
      }
    );
  }
  
  

  

}
