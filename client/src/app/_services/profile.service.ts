import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Profile } from '../_models/profile';
import { map, of } from 'rxjs';
import { Sap } from '../_models/sap';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  baseUrl = environment.apiUrl;
  ipProfile: Profile [] = [];
  sapProfile: Sap [] = [];
  constructor(private http: HttpClient) { }

  registerProfile(model: any){
    return this.http.post<Profile>(this.baseUrl + 'ipprofile/register-ip', model);
  }

  getIps(){
    if (this.ipProfile.length > 0) return of (this.ipProfile);
    return this.http.get<Profile[]>(this.baseUrl + 'ipprofile').pipe(
      map(ipProfile => {
        this.ipProfile = ipProfile;
        return ipProfile;
      })
    )
  }

  getIp(id: number){
    const ip = this.ipProfile.find(x => x.id === id);
    if (ip) return of (ip);
    return this.http.get<Profile>(this.baseUrl + 'ipprofile/' + id);
  }
  
  registerSap(model: any){
    return this.http.post<Sap>(this.baseUrl + 'sapprofile/register-sap', model);
  }

  getSaps(){
    if (this.sapProfile.length > 0) return of (this.sapProfile);
    return this.http.get<Sap[]>(this.baseUrl + 'sapProfile').pipe(
      map(sapProfile => {
        this.sapProfile = sapProfile;
        return sapProfile;
      })
    )
  }

  getSap(id: number){
    const sap = this.sapProfile.find(x => x.id === id);
    if (sap) return of (sap);
    return this.http.get<Sap>(this.baseUrl + 'sapProfile/' + id);
  }

  
}
