import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, of, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Documentation } from '../_models/documentation';

@Injectable({
  providedIn: 'root'
})
export class DocumentationService {

  baseUrl = environment.apiUrl;
  documentation: Documentation [] = [];

  constructor(private http: HttpClient) { }

      // Implement the method to upload documentation
      uploadDocumentation(title: string, description: string, addedDateTime: string, files: File[], pictures: File[], videos: File[]): Observable<any> {
        const formData = new FormData();
        formData.append('title', title);
        formData.append('description', description);
        formData.append('addedDateTime', addedDateTime);

        // Append files, pictures, and videos to the formData
        for (const file of files) {
          formData.append('files', file, file.name);
        }

        for (const picture of pictures) {
          formData.append('pictures', picture, picture.name);
        }

        for (const video of videos) {
          formData.append('videos', video, video.name);
        }

        const headers = new HttpHeaders({ 'Accept': 'application/json' });

        return this.http.post(`${this.baseUrl}documentation/register-documentation`, formData, { headers })
          .pipe(
            catchError(this.handleError)
          );
      }

      // Handle errors
      private handleError(error: HttpErrorResponse) {
        if (error.error instanceof ErrorEvent) {
          // A client-side error occurred.
          console.error('An error occurred:', error.error.message);
        } else {
          // The backend returned an unsuccessful response code.
          console.error(
            `Backend returned code ${error.status}, ` +
            `body was: ${error.error}`
          );
        }
        // Return an observable with a user-facing error message.
        return throwError('Something went wrong; please try again later.');
      }

      // registerDocumentation(model: any){
      //     return this.http.post<Documentation>(this.baseUrl + "documentation/register-documentation", model);
      // }

      public uploadFiles(title: string, description: string, addedDateTime: string, files: File[]) {
        const formData = new FormData();
        formData.append('title', title);
        formData.append('description', description);
        formData.append('addedDateTime', addedDateTime);
      
        for (const file of files) {
          formData.append('files', file, file.name);
        }
      
        return this.http.post(`${this.baseUrl}documentation/register-files`, formData);
      }

      public uploadPictures(title: string, description: string, addedDateTime: string, pictures: File[]) {
        const formData = new FormData();
        formData.append('title', title);
        formData.append('description', description);
        formData.append('addedDateTime', addedDateTime);
      
        for (const file of pictures) {
          formData.append('pictures', file, file.name);
        }
      
        return this.http.post(`${this.baseUrl}documentation/register-pictures`, formData);
      }
      
      public uploadVideos(title: string, description: string, addedDateTime: string, videos: File[]) {
        const formData = new FormData();
        formData.append('title', title);
        formData.append('description', description);
        formData.append('addedDateTime', addedDateTime);
      
        for (const file of videos) {
          formData.append('videos', file, file.name);
        }
      
        return this.http.post(`${this.baseUrl}documentation/register-videos`, formData);
      }


      uploadMedia(
        title: string, 
        description: string, 
        addedDateTime: string, 
        files: File[], 
        pictures: File[], 
        videos: File[]
      ) {
        const formData = new FormData();
        formData.append('title', title);
        formData.append('description', description);
        formData.append('addedDateTime', addedDateTime);
    
        if (files) {
          for (const file of files) {
            formData.append('files', file, file.name);
          }
        }
    
        if (pictures) {
          for (const picture of pictures) {
            formData.append('pictures', picture, picture.name);
          }
        }
    
        if (videos) {
          for (const video of videos) {
            formData.append('videos', video, video.name);
          }
        }
    
        return this.http.post<Documentation>(this.baseUrl+ "documentation/register-media", formData);
      }

      getDocumentations(){
        if (this.documentation.length > 0) return of (this.documentation);
        return this.http.get<Documentation[]>(this.baseUrl + "documentation/get-documentations").pipe(
          map(documentation =>{
            this.documentation = documentation;
            return documentation;
          })
        )
      }

      getDocumentation(id: number){
        const doc= this.documentation.find(x => x.id === id);
        if (doc) return of (doc);
        return this.http.get<Documentation>(this.baseUrl + 'documentation/get-documentation/' + id);
      }
}
