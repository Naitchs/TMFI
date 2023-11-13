import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Profile } from '../_models/profile';
import { Observable, Subject, map, of } from 'rxjs';
import { Sap } from '../_models/sap';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  baseUrl = environment.apiUrl;
  ipProfile: Profile[] = [];
  sapProfile: Sap[] = [];
  deletedIp: any;
  deletedSap: any;

  private ipRegisteredSource = new Subject<Profile>();
  ipRegistered$ = this.ipRegisteredSource.asObservable();

  private sapRegisteredSource = new Subject<Sap>();
  sapRegistered$ = this.sapRegisteredSource.asObservable();

  constructor(private http: HttpClient) { }

  registerProfile(model: any) {
    return this.http.post<Profile>(this.baseUrl + 'ipprofile/register-ip', model).pipe(
      map((ip) => {
        this.ipRegisteredSource.next(ip);
      })
    )   
  }

  proceedRegister(model: any) {
    return this.http.post<Profile>(this.baseUrl + 'ipprofile/register-ip-proceed', model).pipe(
      map((ip) => {
        this.ipRegisteredSource.next(ip);
      })
    )
  }

  getIps(): Observable<Profile[]>{
    if (this.ipProfile.length > 0) return of(this.ipProfile);
    return this.http.get<Profile[]>(this.baseUrl + 'ipprofile').pipe(
      map(ipProfile => {
        this.ipProfile = ipProfile;
        return ipProfile;
      })
    )
  }

  getIp(publicId: string) {
    const ip = this.ipProfile.find(x => x.publicId === publicId);
    if (ip) return of(ip);
    return this.http.get<Profile>(this.baseUrl + 'ipprofile/' + publicId);
  }

  updateIp(ipProfile: Profile, publicId: string) {
    return this.http.put(this.baseUrl + 'ipprofile/update-ip/' + publicId, ipProfile).pipe(
      map(() => {
        const index = this.ipProfile.indexOf(ipProfile);
        this.ipProfile[index] = { ...this.ipProfile[index], ...ipProfile }
      })
    )
  }


  registerSap(model: any) {
    return this.http.post<Sap>(this.baseUrl + 'sapprofile/register-sap', model).pipe(
      map((sap) => {
        this.sapRegisteredSource.next(sap);
      })
    )   
  }

  proceedRegisterSap(model: any) {
    return this.http.post<Sap>(this.baseUrl + 'sapprofile/register-sap-proceed', model).pipe(
      map((sap) => {
        this.sapRegisteredSource.next(sap);
      })
    )   
  }

  getSaps() {
    if (this.sapProfile.length > 0) return of(this.sapProfile);
    return this.http.get<Sap[]>(this.baseUrl + 'sapProfile').pipe(
      map(sapProfile => {
        this.sapProfile = sapProfile;
        return sapProfile;
      })
    )
  }

  getSap(publicId: string) {
    const sap = this.sapProfile.find(x => x.publicId === publicId);
    if (sap) return of(sap);
    return this.http.get<Sap>(this.baseUrl + 'sapProfile/' + publicId);
  }


  updateSap(sapProfile: Sap, publicId: string) {
    return this.http.put(this.baseUrl + 'sapprofile/update-sap/' + publicId, sapProfile).pipe(
      map(() => {
        const index = this.sapProfile.indexOf(sapProfile);
        this.sapProfile[index] = { ...this.sapProfile[index], ...sapProfile }
      })
    )
  }

  deleteIp(id: number){
    return this.http.delete(this.baseUrl + 'ipprofile/delete-ip/' + id).pipe(
      map(ip => {
        this.deletedIp = ip;
        return ip;
      })
    )
  };

  deleteSap(id: number){
    return this.http.delete(this.baseUrl + 'sapprofile/delete-sap/' + id).pipe(
      map(sap => {
        this.deletedSap = sap;
        return sap;
      })
    )
  };



}
