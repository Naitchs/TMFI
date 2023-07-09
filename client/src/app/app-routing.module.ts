import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { IpProfileComponent } from "./ip-profile/ip-profile.component";
import { MembersListComponent } from "./members/members-list/members-list.component";
import { MembersDetailComponent } from "./members/members-detail/members-detail.component";
import { ListsComponent } from "./lists/lists.component";
import { MessagesComponent } from "./messages/messages.component";
import { AuthGuard } from "./_guards/auth.guard";

const routes : Routes = [
  {path: '', component: HomeComponent}, 
  {path: '', 
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MembersListComponent },
      {path: 'members/:id', component: MembersDetailComponent},
      {path: 'list', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
    ]
  },

  {path: '**', component: HomeComponent,  pathMatch: 'full'}
  

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule{

}