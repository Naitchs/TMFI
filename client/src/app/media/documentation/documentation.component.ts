import { Component, OnInit } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { DocumentationService } from 'src/app/_services/documentation.service';


@Component({
  selector: 'app-documentation',
  templateUrl: './documentation.component.html',
  styleUrls: ['./documentation.component.scss']
})
export class DocumentationComponent implements OnInit{

  documentationForm: FormGroup = new FormGroup({});
  validationErrors: any;
  errorMessage: string | null = null;


  files: File[] = [];
  pictures: File[] = [];
  videos: File[] = [];



  constructor(private documentationService: DocumentationService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router, private location: Location
   ) { }


   ngOnInit(): void {
    this.initializeForm();
  }
  
  initializeForm(){
    this.documentationForm = this.fb.group({
      title: ['', Validators.required], // Add Validators.required for required validation
      description: ['']
    })
  }
  


    registerDocumentation(){
      const values = this.documentationForm.value;
        // Get the current datetime in local time
      const now = new Date();
  
      // Convert to Philippine Time (UTC+8)
      const addedDateTime = new Date(now.getTime() + (8 * 60 * 60 * 1000)).toISOString();
      const publicId = '';

      if (this.documentationForm.invalid) {
        this.errorMessage = "Please enter a title";
        return;
      }

      this.documentationService.uploadMedia(
        publicId,
        values.title, values.description, 
        addedDateTime, this.files, this.pictures, 
        this.videos).subscribe(
        () => {
          window.location.reload();
          this.toastr.success('Documentation uploaded successfully', 'Success');
          this.documentationForm.reset();
        },
        (error) => {
          this.validationErrors = error;
          this.toastr.error('Error uploading media', 'Error');
        }
      );
    }
    

onFileChange(event: Event) {
  const inputElement = event.target as HTMLInputElement;
  this.files = Array.from(inputElement.files);
}

onPictureChange(event: Event) {
  const inputElement = event.target as HTMLInputElement;
  this.pictures = Array.from(inputElement.files);
}

onVideoChange(event: Event) {
  const inputElement = event.target as HTMLInputElement;
  this.videos = Array.from(inputElement.files);
}

removeFile(index: number) {
  if (index >= 0 && index < this.files.length) {
    this.files.splice(index, 1); // Remove the file at the specified index
  }
}

removePicture(index: number) {
  if (index >= 0 && index < this.pictures.length) {
    this.pictures.splice(index, 1);
  }
}

removeVideo(index: number) {
  if (index >= 0 && index < this.videos.length) {
    this.videos.splice(index, 1);
  }
}


  

}
