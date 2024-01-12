import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Exceldata } from 'src/app/_models/exceldata';
import { Exceldatarow } from 'src/app/_models/exceldatarow';
import { IntegrationService } from 'src/app/_services/integration.service';
import { environment } from 'src/environments/environment';
declare var $: any;

@Component({
  selector: 'app-excel-upload-file',
  templateUrl: './excel-upload-file.component.html',
  styleUrls: ['./excel-upload-file.component.scss']
})
export class ExcelUploadFileComponent {


  baseUrl = environment.apiUrl;
  excelData: Exceldatarow[] = [];
  title: string;
  selectedSheet: string = '';
  sheetNames: string[] = [];
  excelDataMap: { [key: string]: Exceldatarow[] } = {};
  showSaveButton: boolean = false;
  integrationForm: FormGroup = new FormGroup({});
  validationErrors: any = null;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  publicId: string;
  showUploadButton: boolean = false;


  constructor(private integrationService: IntegrationService, private http: HttpClient, private toastr: ToastrService, private fb: FormBuilder) { }


  fileToUpload: File | null = null;

  handleFileInput(files: FileList) {
    const file = files.item(0);
    if (file) {
      const fileExtension = file.name.split('.').pop()?.toLowerCase(); // Kunin ang extension ng file
      if (fileExtension !== 'xlsx') {
        this.errorMessage = 'Invalid file extension. Allowed extension is .xlsx';
        return;
      }

      this.fileToUpload = file;

      this.errorMessage = null; // I-reset ang errorMessage kung walang error
      this.uploadFileToBackend();
    } else {
      this.errorMessage = 'No file selected';
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleFileInput(files);
    }
  }


  uploadFileToBackend() {
    if (!this.fileToUpload) {
      this.errorMessage = "No file selected or Invalid file extension";
      return;
    }
    const formData = new FormData();
    formData.append('file', this.fileToUpload as File);

    formData.append('fileToUpload', this.fileToUpload);

    this.http.post(`${this.baseUrl}integrate/upload-excel`, formData)
      .subscribe((response: { [key: string]: Exceldatarow[] }) => {
        // console.log(response);
        this.fileToUpload = this.fileToUpload;
        this.sheetNames = Object.keys(response);
        this.selectedSheet = this.sheetNames[0];
        this.excelDataMap = response;
        this.showSaveButton = true;
        this.showUploadButton = true;
        // this.validationErrors = null; // I-reset ang validationErrors
      }, (error) => {
        // console.error(error);
        this.validationErrors = error;
        this.toastr.error('Error uploading Excel', 'Error');
      });
  }


  selectSheet(sheetName: string) {
    this.selectedSheet = sheetName;
  }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.integrationForm = this.fb.group({
      title: ['', Validators.required],
      dateFrom: ['', Validators.required],
      dateTo: ['', Validators.required]
    })
  }

  modalHide() {
    $('#confirmationModal').modal('hide');
    $('#proceedModal').modal('hide');
  }

  saveExcelData() {
    $('#proceedModal').modal('hide');
    if (!this.fileToUpload) {
      console.error("No file selected");
      return;
    }
    const values = this.integrationForm.value;
    const now = new Date();
    const dateUploaded = new Date(now.getTime() + (8 * 60 * 60 * 1000)).toISOString();
    const publicId = '';

    if (this.integrationForm.invalid) {
      this.errorMessage = "Please enter a title";
      return;
    }

    const dateFrom = new Date(values.dateFrom).toISOString();
    const dateTo = new Date(values.dateTo).toISOString();


    const formData = new FormData();
    formData.append('publicId', publicId);
    formData.append('title', values.title);
    formData.append('dateFrom', dateFrom);
    formData.append('dateTo', dateTo);
    formData.append('dateUploaded', dateUploaded);
    formData.append('excelFile', this.fileToUpload, this.fileToUpload.name);


    this.http.post(`${this.baseUrl}integrate/save-excel-data`, formData)
      .subscribe(
        response => {
          // this.toastr.success('Excel uploaded successfully', 'Success');
          this.successMessage = 'Excel uploaded successfully'
        },
        error => {
          this.validationErrors = error;
          // this.toastr.error('Error uploading Excel', 'Error');
          this.errorMessage = 'Error uploading Excel';
        }
      );
  }



}
