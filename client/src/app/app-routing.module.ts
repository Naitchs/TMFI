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
import { PreventUnsavedChangesGuard } from "./_guards/prevent-unsaved-changes.guard";
import { AdminPanelComponent } from "./admin/admin-panel/admin-panel.component";
import { AdminGuard } from "./_guards/admin.guard";
import { ProfilingPanelComponent } from "./profiling/profiling-panel/profiling-panel.component";
import { IpDetailComponent } from "./profiling/ip-detail/ip-detail.component";
import { SapDetailComponent } from "./profiling/sap-detail/sap-detail.component";
import { DocumentationPanelComponent } from "./media/documentation-panel/documentation-panel.component";
import { DocumentationDetailComponent } from "./media/documentation-detail/documentation-detail.component";


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
      {path: 'ip-detail/:id', component: IpDetailComponent},
      {path: 'sap-detail/:id', component: SapDetailComponent},
      {path: 'documentation-detail/:id', component: DocumentationDetailComponent},
      {path: 'profiling', component: ProfilingPanelComponent},
      {path: 'documentation', component: DocumentationPanelComponent},
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