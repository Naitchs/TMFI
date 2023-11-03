import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, NgForm, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit{

  @ViewChild('editForm') editForm: NgForm |undefined;
  @ViewChild('passwordForm') passwordForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if (this.editForm?.dirty){
      $event.returnValue = true;
    }
  }
  member: Member | undefined;
  user: User | null = null;

  constructor (private accountService: AccountService, private memberService: MembersService,
            private toastr: ToastrService, private fb: FormBuilder) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    })
  }

  updateMember(){
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      }
    })
  }

  // changePassword() {
  //   if (this.passwordForm && this.passwordForm.valid) {
  //     const currentPassword = this.passwordForm.controls['currentPassword'].value;
  //     const newPassword = this.passwordForm.controls['newPassword'].value;

  //     this.memberService.changePassword(currentPassword, newPassword).subscribe({
  //       next: _ => {
  //         this.toastr.success('Password updated successfully');
  //         this.passwordForm?.reset();
  //       },
  //       error: err => {
  //         this.toastr.error(err);
  //       }
  //     });
  //   }
  // }

 
}
