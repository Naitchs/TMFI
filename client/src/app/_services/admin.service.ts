import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles(){
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(username: string, roles: any[]){
    return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/' 
        + username + '?roles=' + roles, {});
  }
  
  updateUserActiveStatus(username: string, activeStatus: string): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    const body = JSON.stringify(activeStatus);
  
    return this.http.put<any>(`${this.baseUrl}admin/edit-active-status/${username}`, body, { headers });
  }
  
}
