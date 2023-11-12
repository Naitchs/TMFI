import { Component, HostListener, ViewChild } from '@angular/core';
import { FormBuilder, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Sap } from 'src/app/_models/sap';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProfileService } from 'src/app/_services/profile.service';
declare var $: any;


@Component({
  selector: 'app-sap-edit',
  templateUrl: './sap-edit.component.html',
  styleUrls: ['./sap-edit.component.scss']
})
export class SapEditComponent {


  @ViewChild('editForm') editForm: NgForm |undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
    if (this.editForm?.dirty){
      $event.returnValue = true;
    }
  }
  sapProfile: Sap | undefined;
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
      this.loadSap(this.publicId);
    }
  });
}


loadSap(publicId: string) {
  this.profileService.getSap(publicId).subscribe({
    next: sapProfile => {
      console.log(sapProfile);
      this.sapProfile = sapProfile;
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

updateSap(){
  $('#proceedModal').modal('hide');
  if (this.editForm && this.editForm.form.valid) {
    const updatedSapProfile: Sap = { ...this.sapProfile, ...this.editForm.value };
    this.profileService.updateSap(updatedSapProfile, this.publicId!).subscribe({
      next: () => {
        this.successMessage = 'Sap updated successfully';
        this.editForm?.reset(this.sapProfile);
        console.log(this.sapProfile);
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
