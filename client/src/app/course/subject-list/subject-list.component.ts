import { Component, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subjects } from 'src/app/_models/subject';
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
    this.router.navigate(['/subject-detail', id]);
  }

  redirectToEdit(id: number) {
    this.router.navigate(['/subject-edit', id]);
  }

  deleteSubject() {
    $('#proceedModal').modal('hide');
    console.log(this.subjectId);
    if (this.subjectId == 0) return;
    this.courseService.deleteSubject(this.subjectId).subscribe(
      (subject) => {
        this.successMessage = 'Deleted Successfully!';
        this.subjectId = 0;

        // Find the index of the deleted subject in the array
        const index = this.subjects.findIndex(c => c.id === this.subjectId);

        if (index !== -1) {
          // Remove the deleted subject from the array
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
      phase: [Validators.required],
    });
  }

  createSubject() {
    const values = {...this.subjectForm.value};

    if (this.subjectForm.invalid) {
      this.errorMessageModal = "Please fill up all the required";
      return;
    }

    this.courseService.addSubject(values).subscribe(
      (subjects) => {
        this.successMessage = 'Course Added Successfully!';
        this.subjectForm.reset();
        this.errorMessage = null;
        this.errorMessageModal = null;
        // console.log("----------", subjects);
        this.subjects.push(subjects);
        $('#addSubjectModal').modal('hide');


      }, (error) => {
        this.errorMessageModal = 'Error';
        this.successMessage = null;
      }
    )
  }


  get paginatedList() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.subjects.slice(startIndex, endIndex);
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  get totalPages() {
    return Math.ceil(this.subjects.length / this.itemsPerPage);
  }

}
