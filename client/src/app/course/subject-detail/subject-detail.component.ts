import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PhaseEnum, Subjects } from 'src/app/_models/subject';
import { CourseService } from 'src/app/_services/course.service';

@Component({
  selector: 'app-subject-detail',
  templateUrl: './subject-detail.component.html',
  styleUrls: ['./subject-detail.component.scss']
})
export class SubjectDetailComponent {

  subject: Subjects | undefined;
  id: number | undefined;
  phases: string[] = Object.keys(PhaseEnum).filter(key => isNaN(Number(PhaseEnum[key])));

  constructor(private courseService: CourseService, private route: ActivatedRoute,
    private router: Router) { }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const encryptedId = params['id'];

      if (encryptedId) {
        this.id = +atob(encryptedId)
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

  getPhaseName(phase: PhaseEnum): string {
    return PhaseEnum[phase];
  }

  redirectToEdit(id: number) {
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/subject-edit', encryptedId]);
  }


}

