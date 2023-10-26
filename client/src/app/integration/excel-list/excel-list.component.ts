// import { Component, Input, OnInit, Renderer2 } from '@angular/core';
// import { Router } from '@angular/router';
// import { Exceldata } from 'src/app/_models/exceldata';
// import { IntegrationService } from 'src/app/_services/integration.service';

// @Component({
//   selector: 'app-excel-list',
//   templateUrl: './excel-list.component.html',
//   styleUrls: ['./excel-list.component.scss']
// })
// export class ExcelListComponent implements OnInit{
 
//   @Input() excelDatas: Exceldata | undefined;
//   excelData: Exceldata[] = [];

//   currentPage: number = 1;
//   itemsPerPage: number = 8;
//   totalItems: number;

//   constructor(private integrationService: IntegrationService, private router: Router,
//     private renderer: Renderer2) { }

//     redirectToDetail(publicId: string) {
//       this.router.navigate(['/excel-detail', publicId]);
//     }

//     ngOnInit(): void {
//       this.integrationService.getExcelDatas().subscribe(excelData => { // Subscribe to the observable here
//         this.excelData = excelData;
//       });
//     }

    

//     caps(str: string): string {
//       if (!str) return str;
    
//       return str
//         .toLowerCase()
//         .split(' ')
//         .map(word => word.charAt(0).toUpperCase() + word.slice(1))
//         .join(' ');
//     }

//     get paginatedList() {
//       const startIndex = (this.currentPage - 1) * this.itemsPerPage;
//       const endIndex = startIndex + this.itemsPerPage;
//       return this.excelData.slice(startIndex, endIndex);
//     }
  
//     previousPage() {
//       if (this.currentPage > 1) {
//         this.currentPage--;
//       }
//     }
    
//     nextPage() {
//       if (this.currentPage < this.totalPages) {
//         this.currentPage++;
//       }
//     }
    
//     get totalPages() {
//       return Math.ceil(this.excelData.length / this.itemsPerPage);
//     }
  
// }

import { Component, Input, OnInit, Renderer2, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Exceldata } from 'src/app/_models/exceldata';
import { IntegrationService } from 'src/app/_services/integration.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { BsDatepickerDirective } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-excel-list',
  templateUrl: './excel-list.component.html',
  styleUrls: ['./excel-list.component.scss']
})
export class ExcelListComponent implements OnInit{

  @Input() excelDatas: Exceldata | undefined;
  excelData: Exceldata[] = [];
  displayedColumns: string[] = ['id', 'title', 'dateUploaded', 'action'];
  dataSource: MatTableDataSource<Exceldata>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('datepicker', { read: BsDatepickerDirective }) datepicker: BsDatepickerDirective;


  constructor(private integrationService: IntegrationService, private router: Router,
    private renderer: Renderer2) { 
  }

  redirectToDetail(publicId: string) {
    this.router.navigate(['/excel-detail', publicId]);
  }

  ngOnInit(): void {
    this.integrationService.getExcelDatas().subscribe(excelData => {
      this.excelData = excelData;
      this.dataSource = new MatTableDataSource(this.excelData);
      this.dataSource.paginator = this.paginator;
    });
  }

  caps(str: string): string {
    if (!str) return str;

    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  

}
