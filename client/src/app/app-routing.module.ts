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
import { ExcelPanelComponent } from "./integration/excel-panel/excel-panel.component";
import { ExcelDetailComponent } from "./integration/excel-detail/excel-detail.component";
import { IpEditComponent } from "./profiling/ip-edit/ip-edit.component";
import { SapEditComponent } from "./profiling/sap-edit/sap-edit.component";
import { CoursePanelComponent } from "./course/course-panel/course-panel.component";
import { SubjectPanelComponent } from "./course/subject-panel/subject-panel.component";
import { UpdateCourseComponent } from "./course/update-course/update-course.component";
import { CourseDetailComponent } from "./course/course-detail/course-detail.component";
import { EnrollStudentsComponent } from "./course/enroll-students/enroll-students.component";
import { AttendanceComponent } from "./course/attendance/attendance.component";
import { HeroesComponent } from "./heroes/heroes.component";
import { UpdateSubjectComponent } from "./course/update-subject/update-subject.component";
import { SubjectDetailComponent } from "./course/subject-detail/subject-detail.component";
import { SubjectListComponent } from "./course/subject-list/subject-list.component";
import { CertPanelComponent } from "./hr/certificate/cert-panel/cert-panel.component";
import { PanelComponent } from "./hr/certificate/board-resolution/panel/panel.component";
import { UploadBrComponent } from "./hr/certificate/board-resolution/upload-br/upload-br.component";


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

      {path: 'ip-profiling', component: ProfilingPanelComponent},
      {path: 'ip-detail/:publicId', component: IpDetailComponent},
      {path: 'ip-edit/:publicId', component: IpEditComponent, canDeactivate: [IpPreventUnsavedChangesGuard]},
      {path: 'ip-panel', component: ProfilingPanelComponent},

      {path: 'sap-profiling', component: SapPanelComponent},
      {path: 'sap-detail/:publicId', component: SapDetailComponent},
      {path: 'sap-edit/:publicId', component: SapEditComponent, canDeactivate: [SapPreventUnsavedChangesGuard]},
      {path: 'sap-panel', component: SapPanelComponent},


      {path: 'documentation-detail/:publicId', component: DocumentationDetailComponent},
      {path: 'documentation', component: DocumentationPanelComponent},
      {path: 'media-panel', component: DocumentationPanelComponent},

      
      {path: 'integration', component: ExcelPanelComponent},
      {path: 'excel-detail/:publicId', component: ExcelDetailComponent},
      {path: 'excel-panel', component: ExcelPanelComponent},

      {path: 'cert-panel', component: CertPanelComponent},

      //board resolution
      {path: 'panel-br', component: PanelComponent},
      {path: 'upload-br', component: UploadBrComponent},
   
   
      {path: 'subject', component: SubjectPanelComponent},
      {path: 'subject-detail/:id', component: SubjectDetailComponent},
      {path: 'subject-edit/:id', component: UpdateSubjectComponent},
      {path: 'subject-list', component: SubjectListComponent},
      {path: 'subject-panel', component: SubjectPanelComponent},


      {path: 'course', component: CoursePanelComponent},
      {path: 'course-detail/:id', component: CourseDetailComponent},
      {path: 'course-edit/:id', component: UpdateCourseComponent},
      {path: 'course-enroll/:id', component: EnrollStudentsComponent},
      {path: 'course-panel', component: CoursePanelComponent},

      { path: 'attendance/:courseId/subject/:subjectId', component: AttendanceComponent },


      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
      {path: 'home', component: HomeComponent }
    ]
  },
  // {path: 'errors', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'log-in', component: HeroesComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent,  pathMatch: 'full'}
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule{

}