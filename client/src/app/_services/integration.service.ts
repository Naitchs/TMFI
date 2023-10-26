import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Exceldata } from '../_models/exceldata';

@Injectable({
  providedIn: 'root'
})
export class IntegrationService {



  baseUrl = environment.apiUrl;
  excelData: Exceldata [] = [];
  constructor(private http: HttpClient) { }

// saveExcelData(excelData: any): Observable<any> {
//   return this.http.post<any>(`${this.baseUrl}integrate/save-excel-data`, excelData);
// }

saveExcelData(publicId: string, title: string, dateUploaded: string, file: File) {
  const formData = new FormData();
  formData.append('publicId', publicId);
  formData.append('title', title);
  formData.append('dateUploaded', dateUploaded);
  formData.append('file', file, file.name);

  return this.http.post<Exceldata>(`${this.baseUrl}integrate/save-excel-data`, formData);
}


// getExcelData(): Observable<Exceldata[]> {
//   return this.http.get<Exceldata[]>(`${this.baseUrl}integrate/get-exceldata`);
// }

getExcelDatas(){
  if (this.excelData.length > 0) return of (this.excelData);
  return this.http.get<Exceldata[]>(this.baseUrl + "integrate/get-exceldata").pipe(
    map(excelData =>{
      this.excelData = excelData;
      return excelData;
    })
  )
}

getExcelData(publicId: string){
  const edata = this.excelData.find(x => x.publicId === publicId);
  if (edata) return of (edata);
  return this.http.get<Exceldata>(this.baseUrl + 'integrate/get-exceldata/' + publicId);
}
}