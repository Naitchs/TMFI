import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Course } from 'src/app/_models/course';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;

@Component({
  selector: 'app-course-list',
  templateUrl: './course-list.component.html',
  styleUrls: ['./course-list.component.scss']
})
export class CourseListComponent implements OnInit{

  course: Course [] = [];
  courseId:number;

  successMessage: string | null = null;
  errorMessage: string | null = null;
  constructor(private courseService: CourseService, private router: Router) { }

  ngOnInit(): void { 
    this.courseService.getCourses().subscribe(course => { // Subscribe to the observable here
      this.course = course;
      // console.log(this.course);
    });
    this.courseId = 0;
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
  updateClick(id:number){
    this.courseId = id;
  }

  deleteClick(id:number){
    this.courseId = id;
  }

  redirectToDetail(id: number) {
    this.router.navigate(['/course-detail', id]);
  }

  redirectToEdit(id: number) {
    this.router.navigate(['/course-edit', id]);
  }

  deleteCourse(){
    $('#proceedModal').modal('hide');
    console.log(this.courseId);
    if(this.courseId == 0) return ;
     this.courseService.deleteCourse(this.courseId).subscribe(
      course => {
        this.successMessage = 'Deleted Successfully!';
        this.courseId = 0;
      // Find the index of the deleted course in the array
      const index = this.course.findIndex(c => c.id === this.courseId);

      if (index !== -1) {
        // Remove the deleted course from the array
        this.course.splice(index, 1);
      }
      },(error) => {
        this.errorMessage = 'Failed to delete';
        this.courseId = 0;
      }
     )
  }




  modalHide(){
    $('#proceedModal').modal('hide');
  }

}
