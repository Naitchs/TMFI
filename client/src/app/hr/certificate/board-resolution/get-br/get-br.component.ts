import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GetCertDto } from 'src/app/_models/hr-models/get-cert';
import { HrService } from 'src/app/_services/hr.service';
declare var $: any;

@Component({
  selector: 'app-get-br',
  templateUrl: './get-br.component.html',
  styleUrls: ['./get-br.component.scss']
})
export class GetBrComponent {

  certBr: GetCertDto[] = []
  certId: number;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(private hrService: HrService, private router: Router) { }

  ngOnInit(): void {
    this.getBrs();
    this.certId = 0;
  }

  getBrs() {
    this.hrService.getAllBoardResolutionCerts().subscribe(br => { 
      this.certBr = br;
      // console.log(this.certBr);
    });
  }

  deleteClick(id: number) {
    this.certId = id;
  }

  deleteCert() {
    $('#proceedModal').modal('hide');

    const index = this.certBr.findIndex(c => c.id === this.certId);

    if (this.certId == 0) return;

    this.hrService.deleteCertificate(this.certId).subscribe(
      cert => {
        this.successMessage = 'Deleted Successfully!';
        this.certId = 0;

        if (index !== -1) {
          this.certBr.splice(index, 1);
        }
      }, (error) => {
        this.errorMessage = 'Failed to delete';
        this.certId = 0;
      }
    )
  }

}
