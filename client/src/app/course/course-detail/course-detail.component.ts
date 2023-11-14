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
    this.router.navigate(['/course-edit', id]);
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
              }
            });
            
            this.successMessage = 'Subjects added successfully';
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
    this.show = true;
  }
  back() {
    this.show = false;
    this.showAfterAdd = false;
  }

  getSubjectIdToDelete(id: number) {
    this.subjectId = id;
  }

  modalHide() {
    $('#deleteModal').modal('hide');
  }


  removeSubjectsFromCourse(): void {

    $('#deleteModal').modal('hide');
    
    const subjectIdToRemove: number = this.subjectId;

    if (this.subjectId == 0) return;
    const index = this.subjectsCourse.findIndex(cs => cs.subjectId === subjectIdToRemove);

      this.courseService.removeSubjectsFromCourse(this.id, this.subjectId).subscribe(
        (subject) => {
          this.successMessage = 'Subjects removed from course successfully';
          // Optionally, you can reload the subjects in the course after removal
          // this.loadSubjectsInCourse(this.course.id);
          this.route.params.subscribe(params => {
            this.id = +params['id'];
            if (this.id) {
              this.loadSubjectsInCourse(this.id);
              this.loadSubjectsNotInCourse(this.id);
            }
          });
          if (index !== -1) {
            this.subjectsCourse.splice(index, 1);
          }
        },
        (error) => {
          this.errorMessage = 'Error removing subjects from course:'
        }
      );
    
  }



}
