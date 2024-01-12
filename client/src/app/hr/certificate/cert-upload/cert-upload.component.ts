import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HrService } from 'src/app/_services/hr.service';

@Component({
  selector: 'app-cert-upload',
  templateUrl: './cert-upload.component.html',
  styleUrls: ['./cert-upload.component.scss']
})
export class CertUploadComponent {

  validationErrors: any = null;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  fileToUpload: File[] = [];
  certForm: FormGroup = new FormGroup({});

  constructor(private hrService: HrService,  private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.certForm = this.fb.group({
      title: ['', Validators.required], // Add Validators.required for required validation
      certType: ['', Validators.required ],
      fileToUpload: ['', Validators.required ]
    })
  }

  handleFileInput(files: FileList): void {
    if (files.length === 0) {
      this.errorMessage = 'No files selected';
      return;
    }

    for (let i = 0; i < files.length; i++) {
      const file = files.item(i);
      if (file) {
        const fileExtension = file.name.split('.').pop()?.toLowerCase(); // Get the file extension

        // Check if the file extension is either .pdf or .docx
        if (fileExtension !== 'pdf' && fileExtension !== 'docx') {
          this.errorMessage = 'Invalid file extension. Allowed extensions are .pdf and .docx';
          return;
        }

        // Append the file to the array
        this.fileToUpload.push(file);
      }
    }

    this.errorMessage = null;
  }



  onFileUpload(): void {
    const values = this.certForm.value;
    const formData = new FormData();

    const currentDate = new Date();
    // Set the time zone offset to UTC+8 (Philippine Standard Time)
    const offset = 8 * 60; // 8 hours offset in minutes
    const localDate = new Date(currentDate.getTime() + offset * 60 * 1000);

    const uploadDate = localDate.toISOString();

    formData.append('Title', values.title);
    formData.append('CertType', values.certType);
    formData.append('UploadDate', uploadDate);

    // Append each file from the fileToUpload array
    for (let i = 0; i < this.fileToUpload.length; i++) {
      formData.append('certFile', this.fileToUpload[i], this.fileToUpload[i].name);
    }

    if (this.certForm.invalid) {
      this.errorMessage = "Please enter a fill up all the required inputs";
      return;
    }

    this.hrService.saveCertFile(formData).subscribe(
      response => {
        this.successMessage = 'Certificate saved successfully';
      },
      error => {
        console.error('Error saving certificate', error);
        this.errorMessage = 'Error saving certificate';
      }
    );
  }

  removeFile(index: number) {
    if (index >= 0 && index < this.fileToUpload.length) {
      this.fileToUpload.splice(index, 1); // Remove the file at the specified index
    }
  }



}
