import { Component, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PhaseEnum, Subjects } from 'src/app/_models/subject';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;

@Component({
  selector: 'app-update-subject',
  templateUrl: './update-subject.component.html',
  styleUrls: ['./update-subject.component.scss']
})
export class UpdateSubjectComponent {

  @ViewChild('editForm') editForm: NgForm | undefined;
  phases: string[] = Object.keys(PhaseEnum).filter(key => isNaN(Number(PhaseEnum[key])));


  subject: Subjects | undefined;
  id: number | undefined;
  successMessage: string | null = null;

  constructor(private courseService: CourseService,
    private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const encryptedId = params['id'];

      // Decrypt the id using base64 decoding
      if (encryptedId) {
        this.id = +atob(encryptedId); // Convert back to number
        this.loadSubject(this.id);
      }
    });
  }

  loadSubject(id: number) {
    this.courseService.getSubject(id).subscribe({
      next: subject => {
        this.subject = subject;
        console.log(this.subject);
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

  updateSubject(){
    $('#proceedModal').modal('hide');
    if (this.editForm && this.editForm.form.valid) {
      const updatedSubject: Subjects = { ...this.subject, ...this.editForm.value };

      this.courseService.updateSubject(updatedSubject, this.id!).subscribe({
        next: () => {
          this.successMessage = 'Course updated successfully';
          this.editForm?.reset(this.subject);
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
    this.router.navigate(['/subject-detail', encryptedId]);
  }

  getPhaseName(phase: PhaseEnum): string {
    return PhaseEnum[phase];
  }

}
