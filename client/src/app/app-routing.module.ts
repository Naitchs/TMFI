import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { MembersListComponent } from "./members/members-list/members-list.component";
import { MembersDetailComponent } from "./members/members-detail/members-detail.component";
import { ListsComponent } from "./lists/lists.component";
import { AuthGuard } from "./_guards/auth.guard";
import { NotFoundComponent } from "./errors/not-found/not-found.component";
import { ServerErrorComponent } from "./errors/server-error/server-error.component";
import { MemberEditComponent } from "./members/member-edit/member-edit.component";
import { IpPreventUnsavedChangesGuard, PreventUnsavedChangesGuard, SapPreventUnsavedChangesGuard } from "./_guards/prevent-unsaved-changes.guard";
import { AdminPanelComponent } from "./admin/admin-panel/admin-panel.component";
import { AdminGuard } from "./_guards/admin.guard";
import { ProfilingPanelComponent } from "./profiling/profiling-panel/profiling-panel.component";
import { IpDetailComponent } from "./profiling/ip-detail/ip-detail.component";
import { SapDetailComponent } from "./profiling/sap-detail/sap-detail.component";
import { DocumentationPanelComponent } from "./media/documentation-panel/documentation-panel.component";
import { DocumentationDetailComponent } from "./media/documentation-detail/documentation-detail.component";
import { SapPanelComponent } from "./profiling/sap-panel/sap-panel.component";
import { ExcelUploadFileComponent } from "./integration/excel-upload-file/excel-upload-file.component";
import { ExcelPanelComponent } from "./integration/excel-panel/excel-panel.component";
import { ExcelDetailComponent } from "./integration/excel-detail/excel-detail.component";
import { IpEditComponent } from "./profiling/ip-edit/ip-edit.component";
import { SapEditComponent } from "./profiling/sap-edit/sap-edit.component";
import { AddCourseComponent } from "./course/add-course/add-course.component";
import { CoursePanelComponent } from "./course/course-panel/course-panel.component";
import { SubjectPanelComponent } from "./course/subject-panel/subject-panel.component";
import { UpdateCourseComponent } from "./course/update-course/update-course.component";
import { CourseDetailComponent } from "./course/course-detail/course-detail.component";
import { EnrollStudentsComponent } from "./course/enroll-students/enroll-students.component";
import { AttendanceComponent } from "./course/attendance/attendance.component";


const routes : Routes = [
  {path: '', component: HomeComponent}, 
  {path: '', 
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MembersListComponent },
      {path: 'members/:username', component: MembersDetailComponent},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'list', component: ListsComponent},
      {path: 'ip-detail/:publicId', component: IpDetailComponent},
      {path: 'ip-edit/:publicId', component: IpEditComponent, canDeactivate: [IpPreventUnsavedChangesGuard]},
      {path: 'sap-detail/:publicId', component: SapDetailComponent},
      {path: 'sap-edit/:publicId', component: SapEditComponent, canDeactivate: [SapPreventUnsavedChangesGuard]},
      {path: 'documentation-detail/:publicId', component: DocumentationDetailComponent},
      {path: 'ip-profiling', component: ProfilingPanelComponent},
      {path: 'sap-profiling', component: SapPanelComponent},
      {path: 'documentation', component: DocumentationPanelComponent},
      {path: 'integration', component: ExcelPanelComponent},
      {path: 'excel-detail/:publicId', component: ExcelDetailComponent},
      {path: 'subject', component: SubjectPanelComponent},
      // {path: 'course-detail/:id', component: },
      // {path: 'subject-edit/:id', component: UpdateCourseComponent},
      {path: 'course', component: CoursePanelComponent},
      {path: 'course-detail/:id', component: CourseDetailComponent},
      {path: 'course-edit/:id', component: UpdateCourseComponent},
      {path: 'course-enroll/:id', component: EnrollStudentsComponent},

      { path: 'attendance/:courseId/subject/:subjectId', component: AttendanceComponent },


      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
      {path: 'home', component: HomeComponent }
    ]
  },
  // {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent,  pathMatch: 'full'}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule{

}