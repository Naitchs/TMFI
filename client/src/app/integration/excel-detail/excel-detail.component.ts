import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Exceldata } from 'src/app/_models/exceldata';
import { Exceldatarow } from 'src/app/_models/exceldatarow';
import { IntegrationService } from 'src/app/_services/integration.service';
import { environment } from 'src/environments/environment';
import axios from 'axios';
import * as XLSX from 'xlsx';

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

  constructor(private integrationService: IntegrationService,
    private route: ActivatedRoute,
    private http: HttpClient,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.publicId = params['publicId'];
      if (this.publicId) {
        this.loadExcelData(this.publicId);
      }
    });
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

        if (Exceldata.excelFiles[0]) {
          const cloudinaryUrl = Exceldata.excelFiles[0].url;
          this.downloadFileFromCloudinary(cloudinaryUrl);
        }
      },
      error: (error) => {
        console.error(error);
      }
    });
  }

  downloadFileFromCloudinary(url: string) {
    axios({
      url: url,
      method: 'GET',
      responseType: 'blob', // Important
    }).then((response) => {
      const blob = new Blob([response.data], { type: response.headers['content-type'] });
      const fileName = 'originalFile.xlsx';

      // Convert blob to File
      const file = new File([blob], fileName, { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

      this.fileToUpload = file;
      this.uploadFileToBackend(); // Upload the file to backend
    });
  }


  // loadSheets() {
  //   this.sheetNames = Object.keys(this.excelDatas?.excelDataRowList || {});
  // }

  selectSheet(sheetName: string) {
    this.selectedSheet = sheetName;
  }

  uploadFileToBackend() {
    this.show = false;

    if (!this.fileToUpload) {
      console.error("No file selected");
      return;
    }


    const formData = new FormData();
    formData.append('file', this.fileToUpload as File);

    this.http.post(`${this.baseUrl}integrate/upload-excel`, formData)
      .subscribe((response: { [key: string]: Exceldatarow[] }) => {
        this.sheetNames = Object.keys(response);
        this.selectedSheet = this.sheetNames[0];
        this.excelDataMap = response;
       
      });
  }

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


}
