import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/_models/course';
import { RemoveSubjectsFromCourse } from 'src/app/_models/remove-subjects-from-course';
import { Subjects } from 'src/app/_models/subject';
import { SubjectsCourse } from 'src/app/_models/subjects-course';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  styleUrls: ['./course-detail.component.scss']
})
export class CourseDetailComponent implements OnInit {


  course: Course | undefined;
  id: number | undefined;
  subjects: Subjects[] = [];
  subjectsInCourse: Subjects[] = [];
  selectedSubjects: number[] = [];
  successMessage: string | null = null;
  errorMessage: string | null = null;
  show: boolean = false;
  showAfterAdd: boolean = false;
  subjectsNotInCourse: Subjects[] = [];
  subjectId: number = 0;
  subjectsCourse: RemoveSubjectsFromCourse [] = [];

  constructor(private courseService: CourseService, private route: ActivatedRoute,
    private router: Router) { }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.id = +params['id'];
      if (this.id) {
        this.loadCourse(this.id);
        this.loadSubjectsInCourse(this.id);
        this.loadSubjectsNotInCourse(this.id);
      }
    });

    // this.courseService.getSubjects().subscribe(subjects => this.subjects = subjects);
    this.courseService.getSubjects().subscribe(subjects => {
      this.subjects = subjects;
      this.selectedSubjects = subjects.map(subject => subject.id);
    });


    this.subjectId = 0;

  }

  redirectToEdit(id: number) {
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/course-edit', encryptedId]);
  }

  redirectToEnroll(id: number){
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/course-enroll', encryptedId]);
  }

  loadCourse(id: number) {
    this.courseService.getCourse(id).subscribe({
      next: course => {
        // console.log(ipProfile);
        this.course = course;
      }
    });
  }

  loadSubjectsInCourse(id: number): void {
    this.courseService.getSubjectsInCourse(id).subscribe({
      next: (subjects) => {
        this.subjectsInCourse = subjects;
      },
      error: (error) => {
        this.errorMessage = 'Error loading subjects in course:';
      },
    });
  }

  loadSubjectsNotInCourse(courseId: number): void {
    this.courseService.getSubjectsNotInCourse(courseId).subscribe({
      next: (subjects) => {
        this.subjectsNotInCourse = subjects;
      },
      error: (error) => {
        this.errorMessage = 'Error loading subjects not in course:';
      },
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

  addSubjectsToCourse(): void {
    this.successMessage = null;
    this.errorMessage = null;
    if (this.id !== undefined) {
      const selectedSubjectIds = Array.from(document.querySelectorAll('input[name=selectedSubjects]:checked')).map((el: HTMLInputElement) => parseInt(el.value));

      const dto: SubjectsCourse = { courseId: this.id, subjectIds: selectedSubjectIds };
      this.courseService.addSubjectsToCourse(dto)
        .subscribe(
          response => {
            this.route.params.subscribe(params => {
              this.id = +params['id'];
              if (this.id) {
                this.loadSubjectsInCourse(this.id);
                this.loadSubjectsNotInCourse(this.id);
                this.successMessage = 'Subjects added successfully';
              }
            });
            

            this.show = false;            
          },
          error => {
            this.errorMessage = 'Failed to AddSubject';
          }
        );
    } else {
      this.errorMessage = 'Course ID is undefined';
    }

  }

  showAddSubject() {
    this.successMessage = null;
    this.errorMessage = null;
    this.show = true;
  }
  back() {
    this.successMessage = null;
    this.errorMessage = null;
    this.show = false;
    this.showAfterAdd = false;
  }

  getSubjectIdToDelete(id: number) {
    this.successMessage = null;
    this.errorMessage = null;
    this.subjectId = id;
  }

  modalHide() {
    this.successMessage = null;
    this.errorMessage = null;
    $('#deleteModal').modal('hide');
  }


  removeSubjectsFromCourse(): void {
    this.successMessage = null;
    this.errorMessage = null;

    $('#deleteModal').modal('hide');

    if (this.subjectId == 0) return;

      this.courseService.removeSubjectsFromCourse(this.id, this.subjectId).subscribe(
        (subject) => {
          this.route.params.subscribe(params => {
            this.id = +params['id'];
            if (this.id) {
              this.loadSubjectsInCourse(this.id);
              this.loadSubjectsNotInCourse(this.id);
              this.successMessage = 'Subjects removed from course successfully';
            }
          });
        },
        (error) => {
          this.errorMessage = 'Error removing subjects from course:'
        }
      );
    
  }

  cancel(){
    this.successMessage = null;
    this.errorMessage = null;
    this.show = false;
  }


  redirectToAttendance(courseId: number, subjectId: number) {
    const encryptedCourseId = btoa(courseId.toString());
    const encryptedSubjectId = btoa(subjectId.toString());
    this.router.navigate(['/attendance', encryptedCourseId, 'subject', encryptedSubjectId]);
  }


}
