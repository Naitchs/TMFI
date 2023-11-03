import { Component, EventEmitter, Output, OnInit} from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit{
  
  @Output() cancelRegister = new EventEmitter();
  // registerMode = false;
  registerForm: FormGroup = new FormGroup({});
  maxDate: Date = new Date();
  validationErrors: string [] | undefined;

  constructor(private accountService: AccountService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router){ }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

    // Validation function for requiring at least one digit
    passwordRequiresDigit(control: AbstractControl): { [key: string]: any } | null {
      const value: string = control.value;
      if (/[0-9]/.test(value)) {
        return null; // Valid
      }
      return { passwordRequiresDigit: true }; // Invalid
    }
  
    // Validation function for requiring at least one uppercase letter
    passwordRequiresUpper(control: AbstractControl): { [key: string]: any } | null {
      const value: string = control.value;
      if (/[A-Z]/.test(value)) {
        return null; // Valid
      }
      return { passwordRequiresUpper: true }; // Invalid
    }

  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      activeStatus: ['active', Validators.required],
      // dateOfBirth: ['', Validators.required],
      // city: ['', Validators.required],
      //  country: ['', 
      //           Validators.required
      //         ],
       password: ['Pa$$w0rd', [
                Validators.required, 
                Validators.maxLength(15), 
                Validators.minLength(8),
                this.passwordRequiresDigit, 
                this.passwordRequiresUpper
                ]
              ],
       confirmPassword: ['Pa$$w0rd', [
                Validators.required, 
                  this.matchValue('password')
                ]
              ]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    });
    // this.registerForm.controls['gender'].valueChanges.subscribe({
    //   next: () => this.registerForm.controls['gender'].updateValueAndValidity()
    // });
  }

  matchValue(matchTo: string): ValidatorFn {
      return (control: AbstractControl) => {
        return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
      }
  }

  register(){
    // console.log(this.registerForm?.value);
    // const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);, dateOfBirth: dob
    const values = {...this.registerForm.value};
    this.accountService.register(values).subscribe({
      next: () => {
        this.router.navigateByUrl('/home')
      },
      error: error => {
        this.validationErrors = error
      }
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined){
    if (!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset()))
    .toISOString().slice(0,10);
  }

}
