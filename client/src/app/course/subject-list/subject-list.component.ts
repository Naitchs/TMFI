import { HttpErrorResponse } from '@angular/common/http';
import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PhaseEnum, Subjects } from 'src/app/_models/subject';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.scss']
})

export class SubjectListComponent {

  // @ViewChild('subjectForm') subjectForm: NgForm | undefined;
  subjectForm: FormGroup = new FormGroup({});
  // phases: PhaseEnum[] = Object.values(PhaseEnum) as PhaseEnum[];
  // phases: string[] = Object.keys(PhaseEnum).filter(key => !isNaN(Number(PhaseEnum[key])));
  phases: string[] = Object.keys(PhaseEnum).filter(key => isNaN(Number(PhaseEnum[key])));

 

  subjects: Subjects[] = [];
  subjectId: number;

  successMessage: string | null = null;
  errorMessage: string | null = null;
  errorMessageModal: string | null = null;

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;

  constructor(private courseService: CourseService, private router: Router, private fb: FormBuilder) { }

  ngOnInit(): void {
    // this.courseId = 0;
    this.getSubjects();
    this.initializeForm();
  }


  getSubjects() {
    this.courseService.getSubjects().subscribe(subject => { // Subscribe to the observable here
      this.subjects = subject;
      // console.log(this.subjects);
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

  deleteClick(id: number) {
    this.subjectId = id;
  }

  redirectToDetail(id: number) {
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/subject-detail', encryptedId]);
  }

  redirectToEdit(id: number) {
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/subject-edit', encryptedId]);
  }

  deleteSubject() {
    $('#proceedModal').modal('hide');
    console.log(this.subjectId);

    const index = this.subjects.findIndex(c => c.id === this.subjectId);

    if (this.subjectId == 0) return;

    this.courseService.deleteSubject(this.subjectId).subscribe(
      (subject) => {
        this.successMessage = 'Deleted Successfully!';
        this.subjectId = 0;

        if (index !== -1) {
          this.subjects.splice(index, 1);
        }
      }, (error) => {
        this.errorMessage = 'Failed to delete';
        this.subjectId = 0;
      }
    )
  }


  modalHide() {
    $('#proceedModal').modal('hide');
  }

  initializeForm() {
    this.subjectForm = this.fb.group({
      subjectCode: ['', Validators.required],
      name: ['', Validators.required],
      description: [''],
      phase: [null, Validators.required],
    });
  }

  createSubject() {
    const values = { ...this.subjectForm.value };

    if (this.subjectForm.invalid) {
      this.errorMessageModal = 'Please fill up all the required fields.';
      return;
    }

    values.phase = +values.phase;

    this.courseService.addSubject(values).subscribe(
      (addedSubject) => {
        this.successMessage = 'Subject added successfully!';
        this.resetForm();
        this.subjects.push(addedSubject);
        $('#addSubjectModal').modal('hide');
      },
      (error) => {
        this.errorMessageModal = 'Error adding subject.';

        if (error instanceof HttpErrorResponse && error.status === 400) {
          // Log the server-side validation error
          console.error('Server-side validation error:', error.error);
        } else {
          console.error('Error:', error);
        }
      }
    );
  }

  resetForm() {
    this.subjectForm.reset({
      phase: null // Resetting the phase to null
    });
  }

  onModalHidden() {
    this.resetForm();
  }

  getPhaseName(phase: PhaseEnum): string {
    return PhaseEnum[phase];
  }
  


  get paginatedList() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.subjects.slice(startIndex, endIndex);
  }

  previousPage() {
    this.errorMessageModal = null;
    this.successMessage = null;
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage() {
    this.errorMessageModal = null;
    this.successMessage = null;
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  get totalPages() {
    return Math.ceil(this.subjects.length / this.itemsPerPage);
  }

}
