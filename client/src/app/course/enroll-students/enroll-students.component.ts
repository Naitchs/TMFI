import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/_models/course';
import { EnrollStudent } from 'src/app/_models/enroll-student';
import { Profile } from 'src/app/_models/profile';
import { CourseService } from 'src/app/_services/course.service';
import { ProfileService } from 'src/app/_services/profile.service';
declare var $: any;

@Component({
  selector: 'app-enroll-students',
  templateUrl: './enroll-students.component.html',
  styleUrls: ['./enroll-students.component.scss']
})
export class EnrollStudentsComponent {

  course: Course | undefined;
  ipProfile: Profile | undefined;
  id: number | undefined;
  studentsNotInCourse: Profile[] = [];
  studentsInCourse: Profile[] = [];
  search: string | null = null;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  show: boolean = false;
  showDetails: boolean = false;
  studentId: number = 0;
  deleteStudent: number = 0;
  searchMessage: string | null = null;

  currentPage: number = 1;
  itemsPerPage: number = 8;
  totalItems: number;

  constructor(private courseService: CourseService, private route: ActivatedRoute,
    private router: Router, private profileService: ProfileService) { }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      if (this.id) {
        this.loadCourse(this.id);
        this.loadStudents(this.id);
      }
    });

    this.studentId = 0;
    this.deleteStudent = 0;
  }

  loadCourse(id: number) {

    this.courseService.getCourse(id).subscribe({
      next: course => {
        this.course = course;
      }
    });
  }

  loadStudents(id: number) {
    this.courseService.getStudentsInCourse(id).subscribe({
      next: students => {
        this.studentsInCourse = students;
      }
    });
  }

  loadStudent(id: number) {
    this.show = false;
    this.profileService.getStudent(id).subscribe({
      next: ipProfile => {
        // console.log(ipProfile);
        this.ipProfile = ipProfile;
        this.showDetails = true;
      }
    });
  }

  modalHide() {
    $('#proceedModal').modal('hide');
  }

  addStudentToCourse(): void {
    $('#proceedModal').modal('hide');
    const dto: EnrollStudent = { courseId: this.id, studentId: this.studentId }; // Replace with your actual data

    this.courseService.addStudentToCourse(dto).subscribe(
      (response) => {
        this.route.params.subscribe(params => {
          this.id = +params['id'];
          if (this.id) {
            this.loadStudents(this.id);
            this.successMessage = 'Student added successfully';
            this.errorMessage = null;
          }
        });

      },
      (error) => {
        this.errorMessage = 'Error adding student to course'
        this.successMessage = null;
      }
    );
  }

  close() {
    this.successMessage = null;
    this.errorMessage = null;
    this.showDetails = false;
  }

  searchStudents(): void {
    this.successMessage = null;
    this.errorMessage = null;
    this.show = true;
    this.courseService.searchStudentsNotInCourse(this.id, this.search)
      .subscribe(
        (students) => {
          this.studentsNotInCourse = students;
          this.errorMessage = null;
        },
        error => {
          // this.studentsNotInCourse = [];
          if (error.status === 400) {
            if (error.error === "No students found, Check if it is already Enrolled.") {
              this.searchMessage = 'No students found, Check if it is already Enrolled.';
              this.studentsNotInCourse = [];
            }
          }
        }
      );
  }

  clickToEnroll(id: number) {
    this.successMessage = null;
    this.errorMessage = null;
    this.studentId = id;
  }

  clickToDelete(id: number) {
    this.successMessage = null;
    this.errorMessage = null;
    this.deleteStudent = id;
  }

  caps(str: string): string {
    if (!str) return str;

    return str
      .toLowerCase()
      .split(' ')
      .map(word => word.charAt(0).toUpperCase() + word.slice(1))
      .join(' ');
  }

  getFormattedDate(dateOfBirth: string): string {
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateOfBirth).toLocaleDateString(undefined, options);
  }

  get paginatedList() {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    return this.studentsInCourse.slice(startIndex, endIndex);
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
    return Math.ceil(this.studentsInCourse.length / this.itemsPerPage);
  }

  deleteModalHide() {
    $('#deleteModal').modal('hide');
  }

  removeStudent(): void {

    $('#deleteModal').modal('hide');

    if (this.deleteStudent === 0) {
      return;
    }

    this.courseService.removeStudentFromCourse(this.id, this.deleteStudent).subscribe(
      _ => {
        this.route.params.subscribe(params => {
          this.id = +params['id'];
          if (this.id) {
            this.loadCourse(this.id);
            this.loadStudents(this.id);
          }
        });
        this.successMessage = 'Student remove Successfully';

        console.log("deletedId", this.deleteStudent);
        console.log("courseId", this.id);
      }, (error) => {
        this.errorMessage = 'Error removing Stundent from course:'
      }
    )

  }

  redirectToDetail(id: number) {
    this.router.navigate(['/course-detail', id]);
  }






}
