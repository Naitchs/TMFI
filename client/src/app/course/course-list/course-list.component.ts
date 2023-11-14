import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Course } from 'src/app/_models/course';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent implements OnInit {

  course: Course[] = [];
  courseId: number;

  courseForm: FormGroup = new FormGroup({});

  successMessage: string | null = null;
  errorMessage: string | null = null;
  errorMessageModal: string | null = null;

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;

  constructor(private courseService: CourseService, private router: Router, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.getCourses();
    this.courseId = 0;
    this.initializeForm();
  }


  initializeForm() {
    this.courseForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      durationInHours: ['', Validators.required],
      tags: [''],
    });
  }

  createCourse() {
    const values = { ...this.courseForm.value };
    this.courseService.addCourse(values).subscribe(
      (course) => {
        this.successMessage = 'Course Added Successfully!';
        this.courseForm.reset();
        this.errorMessage = null;
        this.errorMessageModal = null;
        this.course.push(course);
        $('#addCourseModal').modal('hide');
      }, (error) => {
        this.errorMessageModal = 'Error! Please Try Again!';
        this.successMessage = null;
      }
    )
  }

  getCourses() {
    this.courseService.getCourses().subscribe(course => { // Subscribe to the observable here
      this.course = course;
      // console.log(this.course);
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

  // redirectToDetail(publicId: string) {
  //   this.router.navigate(['/ip-detail', publicId]);
  // }
  updateClick(id: number) {
    this.courseId = id;
  }

  deleteClick(id: number) {
    this.courseId = id;
  }

  redirectToDetail(id: number) {
    this.router.navigate(['/course-detail', id]);
  }

  redirectToEdit(id: number) {
    this.router.navigate(['/course-edit', id]);
  }

  deleteCourse() {
    $('#proceedModal').modal('hide');
    // console.log(this.courseId); // Tiyakin na tama ang this.courseId

    const index = this.course.findIndex(c => c.id === this.courseId);
    // console.log("Index ng tinanggal na course:", index);

    if (this.courseId == 0) return;

    this.courseService.deleteCourse(this.courseId).subscribe(
      course => {
        this.successMessage = 'Deleted Successfully!';
        this.courseId = 0;

        if (index !== -1) {
          this.course.splice(index, 1);
        }
      }, (error) => {
        this.errorMessage = 'Failed to delete';
        this.courseId = 0;
      }
    )
  }


  modalHide() {
    $('#proceedModal').modal('hide');
  }

  get paginatedList() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.course.slice(startIndex, endIndex);
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
    return Math.ceil(this.course.length / this.itemsPerPage);
  }

}
