import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Exceldata } from 'src/app/_models/exceldata';
import { Exceldatarow } from 'src/app/_models/exceldatarow';
import { IntegrationService } from 'src/app/_services/integration.service';
import { environment } from 'src/environments/environment';


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
  errorMessage: string | null = null;
  publicId: string;
  showUploadButton: boolean = false;


  constructor(private integrationService: IntegrationService, private http: HttpClient,private toastr: ToastrService, private fb: FormBuilder) {}


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
  
  initializeForm(){
    this.integrationForm = this.fb.group({
      title: ['', Validators.required], 
    })
  }

  saveExcelData() {
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

    const formData = new FormData();
    formData.append('publicId', publicId);
    formData.append('title', values.title); // I-update ang title base sa kailangan mo
    formData.append('dateUploaded', dateUploaded);
    formData.append('excelFile', this.fileToUpload, this.fileToUpload.name);

    this.http.post(`${this.baseUrl}integrate/save-excel-data`, formData)
      .subscribe(
        response => {
          console.log(response);
          // Dito pwede mo i-handle ang successful response
          this.toastr.success('Excel uploaded successfully', 'Success');
        },
        error => {
          console.error(error);
          this.validationErrors = error;
          this.toastr.error('Error uploading Excel', 'Error');
        }
      );
  }

  // saveExcelData() {
  //   if (!this.fileToUpload) {
  //     console.error("No file selected");
  //     return;
  //   }
  
  //   const values = this.integrationForm.value;
  //   const now = new Date();
  //   const dateUploaded = new Date(now.getTime() + (8 * 60 * 60 * 1000)).toISOString();
  //   const publicId = '';
  
  //   // Dito i-check kung meron nang laman ang this.fileToUpload
  //   if (this.fileToUpload) {
  //     this.integrationService.saveExcelData(publicId, values.title, dateUploaded, this.fileToUpload)
  //     .subscribe((response) => {
  //       console.log(response);
  //       this.toastr.success('Excel uploaded successfully', 'Success');
  //     }, (error) => {
  //       this.validationErrors = error;
  //       this.toastr.error('Error uploading Excel', 'Error');
  //     });
    
  //   } else {
  //     console.error("No file selected"); // Kung wala pa rin laman ang this.fileToUpload
  //   }
  // }
  


  // saveExcelData() {
  //   const values = this.integrationForm.value;
  //   const now = new Date();
  //   const dateUploaded = new Date(now.getTime() + (8 * 60 * 60 * 1000));
  
  //   const excelData: Exceldata = {
  //     publicId: '',
  //     title: values.title,
  //     dateUploaded: dateUploaded,
  //     excelDataRowList: this.excelDataMap[this.selectedSheet]
  //   };
  
  //   this.integrationService.saveExcelData(excelData).subscribe(response => {
  //     this.toastr.success('Excel uploaded successfully', 'Success');
  //   });
  // }

  // saveExcelData() {
  //   if (!this.selectedSheet) {
  //     console.error("No sheet selected");
  //     return;
  //   }
  
  //   const values = this.integrationForm.value;
  //   const now = new Date();
  //   const dateUploaded = new Date(now.getTime() + (8 * 60 * 60 * 1000));
  
  //   const excelData = {
  //     publicId: '',
  //     title: values.title,
  //     dateUploaded: dateUploaded,
  //     excelDataRowList: this.excelDataMap[this.selectedSheet]
  //   };
  
  //   this.integrationService.saveExcelData(excelData).subscribe(response => {
  //     console.log('excelData:', excelData);
  //     this.toastr.success('Excel uploaded successfully', 'Success');
  //   });
  // }
  


  // saveExcelData() {
  //   if (!this.excelDataMap) {
  //     console.error("No Excel data available");
  //     return;
  //   }
  
  //   const values = this.integrationForm.value;
  //   const now = new Date();
  //   const dateUploaded = new Date(now.getTime() + (8 * 60 * 60 * 1000));
  
  //   const allExcelDataRows = [];
  
  //   for (const sheetName in this.excelDataMap) {
  //     if (this.excelDataMap.hasOwnProperty(sheetName)) {
  //       const sheetData = this.excelDataMap[sheetName];
  //       allExcelDataRows.push(...sheetData);
  //     }
  //   }
  
  //   const excelData: Exceldata = {
  //     publicId: '', // Pwedeng ilagay ang appropriate na value dito
  //     title: values.title, // Pwedeng kunin mula sa form o ibang source
  //     dateUploaded: dateUploaded, // Dapat gamitin ang na-convert na date
  //     excelDataRowList:  allExcelDataRows // Dapat mapuno base sa napiling sheet at data
  //   };
  
  //   this.integrationService.saveExcelData(excelData).subscribe(response => {
  //     console.log('excelData:', excelData);
  //     this.toastr.success('Excel uploaded successfully', 'Success');
  //   });
  // }
  
  

//   saveAllSheetData() {
//     const values = this.integrationForm.value;
//     const now = new Date();
//     // Convert to Philippine Time (UTC+8)
//     const dateUploaded = new Date(now.getTime() + (8 * 60 * 60 * 1000));

//     const allExcelDataList: Exceldata[] = [];

//     for (const sheetName in this.excelDataMap) {
//         if (this.excelDataMap.hasOwnProperty(sheetName)) {
//             const sheetData = this.excelDataMap[sheetName];

//             const excelData: Exceldata = {
//                 publicId: '', // Pwedeng ilagay ang appropriate na value dito
//                 title: values.title, // Pwedeng kunin mula sa form o ibang source
//                 dateUploaded: dateUploaded, // Dapat gamitin ang na-convert na date
//                 excelDataRowList:  sheetData // Hindi na natin kailangan i-flatten ang listahan
//             };

//             allExcelDataList.push(excelData);
//         }
//     }

//     this.integrationService.saveExcelData(allExcelDataList).subscribe(response => {
//         this.toastr.success('All Excel data uploaded successfully', 'Success');
//     });
// }
  


    // uploadExcel(){
    //   const values = this.integrationForm.value;

    //   const now = new Date();
  
    //   // Convert to Philippine Time (UTC+8)
    //   const addedDateTime = new Date(now.getTime() + (8 * 60 * 60 * 1000)).toISOString();

    //   const jsonData = JSON.stringify(this.excelDataMap);


    //   this.integrationService.saveExcel(
    //     values.title,
    //     this.publicId, // Include publicId
    //     addedDateTime, // Include dateUploaded
    //     jsonData
    //   ).subscribe(
    //     (response) => {
    //       this.toastr.success('Excel uploaded successfully', 'Success');
    //     },
    //     (error) => {
    //       this.validationErrors = error;
    //       this.toastr.error('Error uploading excel file', 'Error');
    //     }
    //   );
    // }


}
