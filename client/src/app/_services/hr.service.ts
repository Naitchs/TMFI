import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Documentation } from '../_models/documentation';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetCertDto } from '../_models/hr-models/get-cert';

@Injectable({
  providedIn: 'root'
})
export class HrService {

  baseUrl = environment.apiUrl;
  documentation: Documentation [] = [];

  constructor(private http: HttpClient) { }

  saveCertFile(formData: FormData): Observable<any> {
    return this.http.post(this.baseUrl + "integrate/save-certificate" , formData);
  }

  getAllBoardResolutionCerts(): Observable<GetCertDto[]> {
    return this.http.get<GetCertDto[]>(this.baseUrl + "integrate/get-all-board-resolution-cert");
  }

  getAllRetirementFundCerts(): Observable<GetCertDto[]> {
    return this.http.get<GetCertDto[]>(this.baseUrl + "integrate/get-all-retirement-fund-cert");
  }

  getAllEmploymentCerts(): Observable<GetCertDto[]> {
    return this.http.get<GetCertDto[]>(this.baseUrl + "integrate/get-all-employment-cert");
  }

  deleteCertificate(certId: number): Observable<any> {
    return this.http.delete<any>(this.baseUrl +  'integrate/delete-certificate/' + certId);
  }

}
