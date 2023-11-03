import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { IpEditComponent } from '../profiling/ip-edit/ip-edit.component';
import { SapEditComponent } from '../profiling/sap-edit/sap-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  canDeactivate(memberEdit: MemberEditComponent ): boolean  {
    if (memberEdit.editForm?.dirty) {
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
    }
    return true;
  }
}

export class IpPreventUnsavedChangesGuard implements CanDeactivate<IpEditComponent> {
  canDeactivate(ipProfile: IpEditComponent ): boolean  {
    if (ipProfile.editForm?.dirty) {
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
    }
    return true;
  }
}

export class SapPreventUnsavedChangesGuard implements CanDeactivate<SapEditComponent> {
  canDeactivate(sapProfile: SapEditComponent ): boolean  {
    if (sapProfile.editForm?.dirty) {
      return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
    }
    return true;
  }
}


