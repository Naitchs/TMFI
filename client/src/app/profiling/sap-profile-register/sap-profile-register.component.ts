import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ProfileService } from 'src/app/_services/profile.service';
declare var $: any;

@Component({
  selector: 'app-sap-profile-register',
  templateUrl: './sap-profile-register.component.html',
  styleUrls: ['./sap-profile-register.component.scss']
})
export class SapProfileRegisterComponent {

  sapForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string [] | undefined;
  confirmationMessage: string | null = null;
  successMessage: string | null = null;

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
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 12);
    }
  
  

    initializeForm(){
      this.sapForm = this.fb.group({
        firstname: ['', Validators.required],
        middlename: [''],
        lastname: ['', Validators.required],
        suffix: [''],
        gender: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        tribe: ['', Validators.required],
        birthPlaceBrgy: ['', Validators.required],
        birthPlaceCity: ['', Validators.required],
        street: ['', Validators.required],
        city: ['', Validators.required],
        churchName: ['', Validators.required],
        fathername: ['', Validators.required],
        mothername: ['', Validators.required],
        parentOccupation: ['', Validators.required],
        numberOfSibling: [0],
        yearSapStarted: ['', Validators.required],
        gradeLevel: ['', Validators.required],
        schoolName: ['', Validators.required],
        communityInvolvement: [''],
        talent: [''],
        sport: [''],
        ambition: [''],
        motto: ['']
      })
    }

    registerSap(){
      $('#proceedModal').modal('hide');
      const dob = this.getDateOnly(this.sapForm.controls['dateOfBirth'].value);
      const publicId = '';
      const values = {...this.sapForm.value, dateOfBirth: dob, publicId: publicId};
      console.log(values);
      this.profileService.registerSap(values).subscribe(
        () => {
          // window.location.reload();
          // this.toastr.success('Sap registered successfully', 'Success');
          this.sapForm.reset(); 
          this.successMessage = 'Sap registered successfully';
          this.validationErrors = null;
        },
        (error) => {
          this.validationErrors = error;
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
      $('#confirmationModal').modal('hide');
      const dob = this.getDateOnly(this.sapForm.controls['dateOfBirth'].value);
      const publicId = '';
      const values = {...this.sapForm.value, dateOfBirth: dob, publicId: publicId};
      this.profileService.proceedRegisterSap(values).subscribe(
        () => {
          // window.location.reload();
          // this.toastr.success('Profile registered successfully', 'Success');
          this.sapForm.reset(); 
          this.confirmationMessage = null;
          this.successMessage = 'Sap registered successfully';
          this.validationErrors = null;
        },
        (error) => {
          this.validationErrors = error; 
          // this.toastr.error('Error registering profile', 'Error');
          }
      );
    }
}
