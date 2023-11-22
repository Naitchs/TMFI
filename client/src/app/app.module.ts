import { CUSTOM_ELEMENTS_SCHEMA, NgModule  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LoginComponent } from './login/login.component';
import { HeroesComponent } from './heroes/heroes.component';
import { RegisterComponent } from './register/register.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DatePickerComponent } from './_forms/date-picker/date-picker.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { RegisterMemberComponent } from './admin/register-member/register-member.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { ProfilingPanelComponent } from './profiling/profiling-panel/profiling-panel.component';
import { IpProfileComponent } from './profiling/ip-profile/ip-profile.component';
import { IpListComponent } from './profiling/ip-list/ip-list.component';
import { IpDetailComponent } from './profiling/ip-detail/ip-detail.component';
import { SapProfileRegisterComponent } from './profiling/sap-profile-register/sap-profile-register.component';
import { SapListComponent } from './profiling/sap-list/sap-list.component';
import { SapDetailComponent } from './profiling/sap-detail/sap-detail.component';
import { TrenComponent } from './tren/tren.component';
import { DocumentationPanelComponent } from './media/documentation-panel/documentation-panel.component';
import { DocumentationListComponent } from './media/documentation-list/documentation-list.component';
import { DocumentationComponent } from './media/documentation/documentation.component';
import { DocumentationDetailComponent } from './media/documentation-detail/documentation-detail.component';
import { SapPanelComponent } from './profiling/sap-panel/sap-panel.component';
import { ExcelUploadFileComponent } from './integration/excel-upload-file/excel-upload-file.component';
import { ExcelListComponent } from './integration/excel-list/excel-list.component';
import { ExcelPanelComponent } from './integration/excel-panel/excel-panel.component';
import { ExcelDetailComponent } from './integration/excel-detail/excel-detail.component';
import { ChangePasswordComponent } from './members/change-password/change-password.component';
import { IpEditComponent } from './profiling/ip-edit/ip-edit.component';
import { IpPreventUnsavedChangesGuard, SapPreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { SapEditComponent } from './profiling/sap-edit/sap-edit.component';
import { AddCourseComponent } from './course/add-course/add-course.component';
import { CoursePanelComponent } from './course/course-panel/course-panel.component';
import { CourseListComponent } from './course/course-list/course-list.component';
import { AddSubjectComponent } from './course/add-subject/add-subject.component';
import { SubjectPanelComponent } from './course/subject-panel/subject-panel.component';
import { SubjectListComponent } from './course/subject-list/subject-list.component';
import { MaterialModule } from './material.module';
import { UpdateCourseComponent } from './course/update-course/update-course.component';
import { CourseDetailComponent } from './course/course-detail/course-detail.component';
import { EnrollStudentsComponent } from './course/enroll-students/enroll-students.component';
import { AttendanceComponent } from './course/attendance/attendance.component';
import { UpdateSubjectComponent } from './course/update-subject/update-subject.component';
import { SubjectDetailComponent } from './course/subject-detail/subject-detail.component';




@NgModule({
  declarations: [
    AppComponent,
    IpProfileComponent,
    HeaderComponent,
    HomeComponent,
    SidenavComponent,
    DashboardComponent,
    LoginComponent,
    HeroesComponent,
    RegisterComponent,
    MembersListComponent,
    MembersDetailComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DatePickerComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    RegisterMemberComponent,
    RolesModalComponent,
    ProfilingPanelComponent,
    IpListComponent,
    IpDetailComponent,
    SapProfileRegisterComponent,
    SapListComponent,
    SapDetailComponent,
    TrenComponent,
    DocumentationPanelComponent,
    DocumentationListComponent,
    DocumentationComponent,
    DocumentationDetailComponent,
    SapPanelComponent,
    ExcelUploadFileComponent,
    ExcelListComponent,
    ExcelPanelComponent,
    ExcelDetailComponent,
    ChangePasswordComponent,
    IpEditComponent,
    SapEditComponent,
    AddCourseComponent,
    CoursePanelComponent,
    CourseListComponent,
    AddSubjectComponent,
    SubjectPanelComponent,
    SubjectListComponent,
    UpdateCourseComponent,
    CourseDetailComponent,
    EnrollStudentsComponent,
    AttendanceComponent,
    UpdateSubjectComponent,
    SubjectDetailComponent,

  ],
  imports: [
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    // * MATERIAL IMPORTS
    HttpClientModule,
    MaterialModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    IpPreventUnsavedChangesGuard,
    SapPreventUnsavedChangesGuard,
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
