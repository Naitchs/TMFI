import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Exceldata } from '../_models/exceldata';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Injectable({
  providedIn: 'root'
})
export class IntegrationService {



  baseUrl = environment.apiUrl;
  excelData: Exceldata[] = [];
  constructor(private http: HttpClient, private sanitizer: DomSanitizer) { }

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

  getExcelDatas() {
    if (this.excelData.length > 0) return of(this.excelData);
    return this.http.get<Exceldata[]>(this.baseUrl + "integrate/get-exceldata").pipe(
      map(excelData => {
        this.excelData = excelData;
        return excelData;
      })
    )
  }

  getExcelData(publicId: string) {
    const edata = this.excelData.find(x => x.publicId === publicId);
    if (edata) return of(edata);
    return this.http.get<Exceldata>(this.baseUrl + 'integrate/get-exceldata/' + publicId);
  }


  // getExcelFile(fileName: string): Observable<SafeResourceUrl> {

  //   return this.http.get(this.baseUrl + 'integrate/' + fileName, { responseType: 'blob' }).pipe(
  //     map((data: Blob) => {
  //       const blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  //       const fileUrl = URL.createObjectURL(blob);
  //       return this.sanitizer.bypassSecurityTrustResourceUrl(fileUrl);
  //     })
  //   );
  // }


  // getExcelFiles(): Observable<any[]> {
  //   const apiUrl = 'https://your-api-base-url/api/excel'; // Update with your actual API URL

  //   return this.http.get(apiUrl).pipe(
  //     map((files: any[]) => {
  //       return files.map(file => {
  //         return {
  //           ...file,
  //           url: this.getExcelFileUrl(file.publicId)
  //         };
  //       });
  //     })
  //   );
  // }

  // private getExcelFileUrl(publicId: string): string {
  //   return `https://your-api-base-url/api/excel/${publicId}`;
  // }

  getExcelFileUrl(publicId: string, fileName: string): string {
    return `${this.baseUrl}integrate/${publicId}?fileName=${encodeURIComponent(fileName)}`;
  }

  getExcelFile(publicId: string): Observable<Blob> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.get(this.baseUrl + 'integrate/' + publicId, { responseType: 'blob', headers });
  }
  


}