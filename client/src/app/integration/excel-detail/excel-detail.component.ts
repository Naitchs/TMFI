import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, SecurityContext, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Exceldata } from 'src/app/_models/exceldata';
import { Exceldatarow } from 'src/app/_models/exceldatarow';
import { IntegrationService } from 'src/app/_services/integration.service';
import { environment } from 'src/environments/environment';
import axios from 'axios';
import * as XLSX from 'xlsx';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-excel-detail',
  templateUrl: './excel-detail.component.html',
  styleUrls: ['./excel-detail.component.scss']
})
export class ExcelDetailComponent implements OnInit {

  baseUrl = environment.apiUrl;
  excelDatas: Exceldata | undefined;
  selectedSheet: string = '';
  publicId: string | undefined;
  sheetNames: string[] = [];
  fileToUpload: File | null = null;
  excelDataMap: { [key: string]: Exceldatarow[] } = {};
  show: boolean = true;
  // excelFileUrl: SafeResourceUrl;
  excelFileUrl: string;
  excelFiles: any[];
  excelData: any[] = [];

  constructor(private integrationService: IntegrationService,
    private route: ActivatedRoute,
    private http: HttpClient,
    private toastr: ToastrService, private sanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.publicId = params['publicId'];
      if (this.publicId) {
        this.loadExcelData(this.publicId);
      }
    });
    // const fileName = this.route.params['fileName'];
    // this.getExcelFile(fileName);

  }

  // loadExcelData(publicId: string) {
  //   this.integrationService.getExcelData(publicId).subscribe({
  //     next: Exceldata => {
  //       this.excelDatas = Exceldata; // Assign to documentationList
  //       // this.sheetNames = Object.keys(Exceldata);
  //       if (Exceldata.excelFiles[0]) {
  //         this.fileToUpload = new File([Exceldata.excelFiles[0]], Exceldata.excelFiles[0].publicId, { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  //         console.log("sulod sa loadexcel",this.fileToUpload);
  //       }
  //       console.log(this.excelDatas);
  //     },
  //     error: (error) => {
  //       console.error(error); // Handle errors here
  //     }
  //   });
  // }

  loadExcelData(publicId: string) {
    this.integrationService.getExcelData(publicId).subscribe({
      next: Exceldata => {
        this.excelDatas = Exceldata;
        // if (Exceldata.excelFiles[0]) {
        //   const cloudinaryUrl = Exceldata.excelFiles[0].url;
        //   this.downloadFileFromCloudinary(cloudinaryUrl);
        // }
        console.log(Exceldata);
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

 

  // downloadFileFromCloudinary(url: string) {
  //   axios({
  //     url: url,
  //     method: 'GET',
  //     responseType: 'blob', // Important
  //   }).then((response) => {
  //     this.show = false;
  //     const blob = new Blob([response.data], { type: response.headers['content-type'] });
  //     const fileName = 'originalFile.xlsx';

  //     // Convert blob to File
  //     const file = new File([blob], fileName, { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

  //     this.fileToUpload = file;
  //     this.uploadFileToBackend(); // Upload the file to backend
  //   });
  // }


  // loadSheets() {
  //   this.sheetNames = Object.keys(this.excelDatas?.excelDataRowList || {});
  // }

  selectSheet(sheetName: string) {
    this.selectedSheet = sheetName;
  }

  // uploadFileToBackend() {
  //   this.show = false;

  //   if (!this.fileToUpload) {
  //     console.error("No file selected");
  //     return;
  //   }


  //   const formData = new FormData();
  //   formData.append('file', this.fileToUpload as File);

  //   this.http.post(`${this.baseUrl}integrate/upload-excel`, formData)
  //     .subscribe((response: { [key: string]: Exceldatarow[] }) => {
  //       this.sheetNames = Object.keys(response);
  //       this.selectedSheet = this.sheetNames[0];
  //       this.excelDataMap = response;
       
  //     });
  // }

  downloadFile(url: string, fileName: string) {
    const link = document.createElement('a');
    link.href = url;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  }


  caps(str: string): string {
    if (!str) return str;

    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

  reload() {
    this.show = false;
    location.reload();
  }

  // getViewerUrl(fileUrl: string, publicId: string): SafeResourceUrl {
  //   this.show = false;
  //   const viewerUrl = `https://view.officeapps.live.com/op/view.aspx?src=${encodeURIComponent(fileUrl + '?publicId=' + publicId)}`;
  //   return this.sanitizer.bypassSecurityTrustResourceUrl(viewerUrl);
  // }

  getViewerUrl(fileName: string, publicId: string): SafeResourceUrl {
    const apiUrl = `${this.baseUrl}integrate/${fileName}?publicId=${publicId}`;
    const viewerUrl = `https://view.officeapps.live.com/op/view.aspx?src=${encodeURIComponent(apiUrl)}`;
    return this.sanitizer.bypassSecurityTrustResourceUrl(viewerUrl);
  }

  // getExcelFile(fileName: string): void {
  //   this.integrationService.getExcelFile(fileName).subscribe(
  //     (safeUrl: SafeResourceUrl) => {
  //       this.excelFileUrl = safeUrl;
  //     },
  //     error => {
  //       console.error('Error fetching Excel file:', error);
  //     }
  //   );
  // }

  private parseExcelFile(file: Blob): Promise<any> {
    const reader = new FileReader();
  
    return new Promise((resolve, reject) => {
      reader.onload = (event: any) => {
        const data = event.target.result;
        const workbook = XLSX.read(data, { type: 'binary' });
  
        // Access the sheet or perform other operations as needed
        const firstSheet = workbook.Sheets[workbook.SheetNames[0]];
        const jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
  
        resolve(jsonData);
      };
  
      reader.onerror = (error) => {
        reject(error);
      };
  
      reader.readAsBinaryString(file);
    });
  }
  
  


  getExcelFiles(publicId: string): void {
    this.integrationService.getExcelFile(publicId).subscribe(
      (file: Blob) => {
        this.parseExcelFile(file).then((jsonData) => {
          console.log(jsonData);
        }).catch((error) => {
          console.error('Error parsing Excel file:', error);
        });
      },
      error => {
        console.error('Error fetching Excel files:', error);
      }
    );
  }
  

  
  


  // viewFile(fileUrl: string, publicId: string) {
  //   const viewerUrl = this.getViewerUrl(fileUrl, publicId);
  //   const newWindow = window.open(viewerUrl);
  //   if (!newWindow) {
  //     console.error('Unable to open a new window for viewing the file.');
  //   }
  // }
  
  


}
