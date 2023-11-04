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
export class DocumentationComponent implements OnInit {

  documentationForm: FormGroup = new FormGroup({});
  validationErrors: any;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  maxFileSize = 5 * 1024 * 1024; // 25 MB

  files: File[] = [];
  pictures: File[] = [];
  videos: File[] = [];



  constructor(private documentationService: DocumentationService, private toastr: ToastrService,
    private fb: FormBuilder, private router: Router, private location: Location
  ) { }


  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.documentationForm = this.fb.group({
      title: ['', Validators.required], // Add Validators.required for required validation
      description: ['']
    })
  }

  clearFileInputs() {
    this.files = []; // Assuming `files`, `pictures`, and `videos` are arrays
    this.pictures = [];
    this.videos = [];

  }

  registerDocumentation() {
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
          this.successMessage = "Documentation uploaded successfully";
          this.documentationForm.reset();
          this.clearFileInputs();
          this.errorMessage = null;
        },
        (error) => {
          this.validationErrors = error;
          this.errorMessage = "Error uploading media";
        }
      );
  }


  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
     this.files = Array.from(input.files);

      for (const file of this.files) {
        if (file.size > this.maxFileSize) {
          this.errorMessage = `File ${file.name} exceeds the maximum allowed size.`;
          this.files = [];
          return;
        }
      }

      this.errorMessage = null;
    }
  }

  onPictureChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.pictures = Array.from(input.files);
  
      for (const picture of this.pictures) {
        const fileExtension = picture.name.split('.').pop()?.toLowerCase();
        if (!['jpg', 'jpeg', 'png', 'gif'].includes(fileExtension)) {
          this.errorMessage = `Invalid picture file extension for ${picture.name}. Allowed extensions are .jpg, .jpeg, .png, .gif`;
          this.pictures = [];
          return;
        }
  
        if (picture.size > this.maxFileSize) {
          this.errorMessage = `Picture ${picture.name} exceeds the maximum allowed size.`;
          this.pictures = [];
          return;
        }
      }
  
      this.errorMessage = null;

    }
  }
  

  onVideoChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.videos = Array.from(input.files);
  
      for (const video of this.videos) {
        const fileExtension = video.name.split('.').pop()?.toLowerCase();
        if (!['mp4', 'webm', 'ogg', 'mp3', 'wav'].includes(fileExtension)) {
          this.errorMessage = `Invalid video/audio file extension for ${video.name}. Allowed extensions are .mp4, .webm, .ogg, 'mp3', 'wav'`;
          this.videos = [];
          return;
        }
  
        if (video.size > this.maxFileSize) {
          this.errorMessage = `Video ${video.name} exceeds the maximum allowed size.`;
          this.videos = [];
          return;
        }
      }
  
      this.errorMessage = null;
    }
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
