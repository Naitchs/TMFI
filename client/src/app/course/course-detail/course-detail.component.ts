import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/_models/course';
import { CourseService } from 'src/app/_services/course.service';

@Component({
  selector: 'app-course-detail',
  templateUrl: './course-detail.component.html',
  styleUrls: ['./course-detail.component.scss']
})
export class CourseDetailComponent implements OnInit{


  course: Course | undefined;
  id: number | undefined;

  constructor(private courseService: CourseService, private route: ActivatedRoute,
    private router: Router) {}


    ngOnInit(): void{
      this.route.params.subscribe(params => {
        this.id = +params['id'];
        if (this.id) {
          this.loadCourse(this.id);
        }
      });
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
  
    caps(str: string): string {
      if (!str) return str;
    
      return str
        .toLowerCase()
        .split(' ')
        .map(word => word.charAt(0).toUpperCase() + word.slice(1))
        .join(' ');
    }
  

}
