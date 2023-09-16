import { CUSTOM_ELEMENTS_SCHEMA, NgModule  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';

import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { SidenavComponent } from './sidenav/sidenav.component';
import { AppRoutingModule } from './app-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatExpansionModule } from '@angular/material/expansion';
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



  ],
  imports: [
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    // * MATERIAL IMPORTS
    MatSidenavModule,
    MatToolbarModule,
    MatMenuModule,
    MatIconModule,
    MatDividerModule,
    MatListModule,
    MatExpansionModule,
    ReactiveFormsModule,

    HttpClientModule,
    SharedModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
