import { Component, AfterViewInit, ElementRef, Renderer2, ViewChild, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ProfileService } from 'src/app/_services/profile.service';
import { Location } from '@angular/common';
import { Profile } from 'src/app/_models/profile';
declare var $: any;




@Component({
  selector: 'app-ip-profile',
  templateUrl: './ip-profile.component.html',
  styleUrls: ['./ip-profile.component.scss']
})
export class IpProfileComponent {

  @Output() ipRegistered: EventEmitter<Profile> = new EventEmitter<Profile>();
  ipForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string [] | undefined;
  confirmationMessage: string | null = null;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  ip: Profile [] = [];


  constructor(private profileService: ProfileService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router, private location: Location, 
    private elementRef: ElementRef, private renderer: Renderer2){ }

  

  private getDateOnly(dob: string | undefined){
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
    .toISOString().slice(0,10);
  }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 12);
  }


  initializeForm(){
    this.ipForm = this.fb.group({
      firstname: ['', Validators.required],
      middlename: [''],
      lastname: ['', Validators.required],
      suffix: [''],
      dateOfBirth: ['', Validators.required],
      gender: ['', Validators.required],
      status: ['', Validators.required],
      tribe: ['', Validators.required],
      phonenum: [''],
      email: [''],
      facebook: [''],
      street: [''],
      barangay: ['', Validators.required],
      city: ['', Validators.required],
      churchname: ['', Validators.required],
      ministry: [''],
      reason: [''],
    });
  }

  registerIp(){
    $('#proceedModal').modal('hide');
    const dob = this.getDateOnly(this.ipForm.controls['dateOfBirth'].value);
    const publicId = '';
    const values = {...this.ipForm.value, dateOfBirth: dob, publicId: publicId};
    this.profileService.registerProfile(values).subscribe(
      (ip) => {
        // window.location.reload();
        // this.toastr.success('Profile registered successfully', 'Success');
        this.ipForm.reset(); 
        this.successMessage = 'Ip registered successfully';
        // this.ip.push(ip);
        // console.log(ip);
        // this.ipRegistered.emit(ip);
        // this.profileService.registerProfile(ip);
      },
      (error) => {
        this.validationErrors = error; // Set the validationErrors array with the error messages
        if (error.status === 400) {
          if (error.error === "An existing record with the same details was found. Do you want to proceed?") {
            this.confirmationMessage = 'An existing record with the same details was found. Do you want to proceed?';
            $('#confirmationModal').modal('show');
          }

        // this.toastr.error('Error registering profile', 'Error');
        }
      }
    );
  }

  modalHide(){
    $('#confirmationModal').modal('hide');
    $('#proceedModal').modal('hide');
  }


  
  proceedRegister(){
    const dob = this.getDateOnly(this.ipForm.controls['dateOfBirth'].value);
    const publicId = '';
    const values = {...this.ipForm.value, dateOfBirth: dob, publicId: publicId};
    this.profileService.proceedRegister(values).subscribe(
      (ip) => {
        // window.location.reload();
        // this.toastr.success('Profile registered successfully', 'Success');
        this.ipForm.reset(); 
        this.confirmationMessage = null;
        $('#confirmationModal').modal('hide');
        this.successMessage = 'Ip registered successfully';
        // this.ip.push(ip);
        // this.ipRegistered.emit(ip);
      },
      (error) => {
        this.validationErrors = error; 
        // this.toastr.error('Error registering profile', 'Error');
        this.errorMessage = 'Error registering profile';
        console.log(error);
        }
    );
  }

  

}
