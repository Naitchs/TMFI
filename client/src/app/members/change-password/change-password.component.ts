import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Changepass } from 'src/app/_models/changepass';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit{
  baseUrl = environment.apiUrl;

  // updatePass: FormGroup = new FormGroup({});
  passwordForm: FormGroup;
  changePass: Changepass | undefined;
  user: User | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor (private accountService: AccountService, private memberService: MembersService,
    private toastr: ToastrService, private fb: FormBuilder) {
this.accountService.currentUser$.pipe(take(1)).subscribe({
next: user => this.user = user
})

}

ngOnInit() {
  this.passwordForm = this.fb.group({
    currentPassword: ['', Validators.required],
    newPassword: ['', [
      Validators.required,
          Validators.maxLength(15),
          Validators.minLength(8),
          this.passwordRequiresDigit,
          this.passwordRequiresUpper
    ]],
    confirmPassword: ['', [
          Validators.required,
          this.matchValue('newPassword')
          ]
        ]
  });
  this.passwordForm.controls['newPassword'].valueChanges.subscribe({
    next: () => this.passwordForm.controls['confirmPassword'].updateValueAndValidity()
  });
}

  validateCurrentPassword(control: AbstractControl): { [key: string]: any } | null {
    const currentPassword: string = control.value;
    if (currentPassword === this.changePass.currentPassword) {
      return null; // Tama ang kasalukuyang password
    }
    return { invalidCurrentPassword: true }; // Hindi tama ang kasalukuyang password
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

  matchValue(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
}


onSubmit() {
  if (this.passwordForm.invalid) {
    return;
  }

  const data = {
    CurrentPassword: this.passwordForm.get('currentPassword').value,
    NewPassword: this.passwordForm.get('newPassword').value,
    ConfirmPassword: this.passwordForm.get('confirmPassword').value
  };

  this.memberService.changePassword(data).subscribe(
    response => {
      this.successMessage = 'Password changed successfully!'
      this.passwordForm.reset();
      this.errorMessage = null;
    },
    error => {
      if (error.status === 400) {
        if (error.error === "Current password is incorrect") {
          this.errorMessage = 'Current password is incorrect';
        } else if (error.error === "New password cannot be the same as the current password") {
          this.errorMessage = 'New password cannot be the same as the current password';
        } else {
          this.errorMessage = 'Error changing password';
        }
      } else {
        this.errorMessage = 'Error changing password';
      }
    }
  );  
}


// async changePassword() {
//   try {
//     const currentPassword = this.updatePass.get('currentPassword')?.value;
//     const newPassword = this.updatePass.get('newPassword')?.value;
  
//     const success = await this.memberService.changePassword({ currentPassword, newPassword }).toPromise();
    
//     if (success) {
//       this.toastr.success('Password changed successfully');
//       this.updatePass.reset(); // I-reset ang form matapos magtagumpay
//     } else {
//       this.toastr.error('Error changing password');
//     }
//   } catch (error) {
//     this.toastr.error('Error changing password');
//   }
// }



}
