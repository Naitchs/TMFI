import { Component, HostListener, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { Course } from 'src/app/_models/course';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;


@Component({
  selector: 'app-update-course',
  templateUrl: './update-course.component.html',
  styleUrls: ['./update-course.component.scss']
})
export class UpdateCourseComponent {

  @ViewChild('editForm') editForm: NgForm |undefined;


  course: Course | undefined;
  id: number | undefined;
  successMessage: string | null = null;
  user: User | null = null;

  constructor (private accountService: AccountService, private courseService: CourseService,
    private route: ActivatedRoute, private router: Router) {
this.accountService.currentUser$.pipe(take(1)).subscribe({
next: user => this.user = user
})
}

  ngOnInit(): void{
    this.route.params.subscribe(params => {
      const encryptedId = params['id'];

      // Decrypt the id using base64 decoding
      if (encryptedId) {
        this.id = +atob(encryptedId); // Convert back to number
        this.loadCourse(this.id);
      }
    });
  }

  loadCourse(id: number) {
    this.courseService.getCourse(id).subscribe({
      next: course => {
        this.course = course;
      }
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

  modalHide(){
    $('#confirmationModal').modal('hide');
    $('#proceedModal').modal('hide');
  }

  updateCourse(){
    $('#proceedModal').modal('hide');
    if (this.editForm && this.editForm.form.valid) {
      const updatedCourse: Course = { ...this.course, ...this.editForm.value };
      this.courseService.updateCourse(updatedCourse, this.id!).subscribe({
        next: () => {
          this.successMessage = 'Course updated successfully';
          this.editForm?.reset(this.course);
          // this.loadIp(this.publicId!);// Reload ang profile
        },
        error: (error) => {
          console.error(error);
          // this.toastr.error('Error updating profile');
        }
      });
    }
  }

  redirectToDetail(id: number) {
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/course-detail', encryptedId]);
  }



}
