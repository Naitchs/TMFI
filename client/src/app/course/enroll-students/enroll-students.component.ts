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
  studentsNotInCourse: Profile [] = [];
  search: string | null = null;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  show: boolean = false;
  showDetails: boolean = false;
  studentId: number = 0;

  constructor(private courseService: CourseService, private route: ActivatedRoute,
    private router: Router, private profileService: ProfileService) { }


    ngOnInit(): void{
      this.route.params.subscribe(params => {
        this.id = +params['id'];
        if (this.id) {
          this.loadCourse(this.id);
        }
      });

      this.studentId = 0;
    }
  
    loadCourse(id: number) {
 
      this.courseService.getCourse(id).subscribe({
        next: course => {
          this.course = course;
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
          this.successMessage = 'Student added successfully';
          this.errorMessage = null;
        },
        (error) => {
          this.errorMessage = 'Error adding student to course'
          this.successMessage = null;
        }
      );
    }

    close(){
      this.showDetails = false;
    }

    searchStudents(): void {
      this.courseService.searchStudentsNotInCourse(this.id, this.search)
        .subscribe(
          students => {
            this.studentsNotInCourse = students;
            this.errorMessage = null;
            this.show = true;
          },
          error => {
            // this.studentsNotInCourse = [];
            this.errorMessage = 'Error fetching students. Please try again.';
          }
        );
    }

    clickToEnroll(id: number){
      this.studentId = id;
      console.log(this.studentId);
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




}
