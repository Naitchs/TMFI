import { Component } from '@angular/core';
import { AddSubjectComponent } from '../add-subject/add-subject.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-subject-panel',
  templateUrl: './subject-panel.component.html',
  styleUrls: ['./subject-panel.component.scss'],
})
export class SubjectPanelComponent {


  constructor(public dialog: MatDialog ) { }

  openAddSubjectDialog(): void {
    const dialogRef = this.dialog.open(AddSubjectComponent);

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }
}
