import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CourseService } from 'src/app/_services/course.service';

@Component({
  selector: 'app-add-subject',
  templateUrl: './add-subject.component.html',
  styleUrls: ['./add-subject.component.scss']
})
export class AddSubjectComponent implements OnInit {

  @ViewChild('subjectForm') subjectForm: NgForm | undefined;
  // subjectForm: FormGroup = new FormGroup({});
  successMessage: string | null = null;
  errorMessage: string | null = null;
  subjectCode: string = '';
  name: string = '';
  description: string = '';
  phase: number;

  constructor(public dialogRef: MatDialogRef<AddSubjectComponent>,
    private course: CourseService) { }

  ngOnInit(): void {
  }

  createSubject() {
    this.course.addSubject(this.subjectForm.value).subscribe(
      () => {
        this.successMessage = 'Course Added Successfully!';
        console.log(this.subjectForm);
        this.dialogRef.close();
      }, (error) => {
        this.errorMessage = 'Error';
      }
    )
  }

  closeDialog(): void {
    this.dialogRef.close();
  }


}