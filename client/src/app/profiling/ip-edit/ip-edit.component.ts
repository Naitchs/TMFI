import { Component, HostListener, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Profile } from 'src/app/_models/profile';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProfileService } from 'src/app/_services/profile.service';
declare var $: any;

@Component({
  selector: 'app-ip-edit',
  templateUrl: './ip-edit.component.html',
  styleUrls: ['./ip-edit.component.scss']
})
export class IpEditComponent {

  @ViewChild('editForm') editForm: NgForm |undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if (this.editForm?.dirty){
      $event.returnValue = true;
    }
  }
  ipProfile: Profile | undefined;
  publicId: string | undefined;
  user: User | null = null;
  successMessage: string | null = null;
  dirtyForm: boolean = false;

  constructor (private accountService: AccountService, private profileService: ProfileService,
    private toastr: ToastrService, private fb: FormBuilder, private route: ActivatedRoute) {
this.accountService.currentUser$.pipe(take(1)).subscribe({
next: user => this.user = user
})
}

ngOnInit(): void{
  this.route.params.subscribe(params => {
    this.publicId = params['publicId'];
    if (this.publicId) {
      this.loadIp(this.publicId);
      this.churchOptions;
      this.statusOptions;
      this.tribeOptions;
    }
  
  });

  this.editForm?.form.get('status')?.valueChanges.subscribe(() => {
    console.log('Status value changed');
    this.editForm?.form.markAsDirty();
  });
  this.editForm?.form.get('tribe')?.valueChanges.subscribe(() => {
    console.log('Tribe value changed');
    this.editForm?.form.markAsDirty();
  });
  this.editForm?.form.get('churchname')?.valueChanges.subscribe(() => {
    console.log('Church Name value changed');
    this.editForm?.form.markAsDirty();
  });
}

loadIp(publicId: string) {
  this.profileService.getIp(publicId).subscribe({
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

updateIp(){
  $('#proceedModal').modal('hide');
  if (this.editForm && this.editForm.form.valid) {
    const updatedProfile: Profile = { ...this.ipProfile, ...this.editForm.value };
    this.profileService.updateIp(updatedProfile, this.publicId!).subscribe({
      next: () => {
        this.successMessage = 'Profile updated successfully';
        this.editForm?.reset(this.ipProfile);
        // this.loadIp(this.publicId!);// Reload ang profile
      },
      error: (error) => {
        console.error(error);
        this.toastr.error('Error updating profile');
      }
    });
  }
}

modalHide(){
  $('#confirmationModal').modal('hide');
  $('#proceedModal').modal('hide');
}


statusOptions = [
  { label: 'Single', value: 'single' },
  { label: 'Married', value: 'married' },
  { label: 'Widow', value: 'widow' },
  { label: 'Deceased', value: 'deceased' }
];

tribeOptions = [
  { label: 'Ata', value: 'ata' },
  { label: 'Maranaw', value: 'maranaw' },
  { label: 'soso', value: 'soso' },
  { label: 'On', value: 'on' }
];

churchOptions = [
  { label: 'San Juan', value: 'San Juan' },
  { label: 'sample', value: 'sample' },
  { label: 'sample1', value: 'ssample' },
  { label: 'sample2', value: 'sample' }
];




}
