import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'src/app/_models/subject';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;

@Component({
  selector: 'app-subject-list',
  templateUrl: './subject-list.component.html',
  styleUrls: ['./subject-list.component.scss']
})

export class SubjectListComponent {

  subject: Subject [] = [];
  subjectId:number;

  successMessage: string | null = null;
  errorMessage: string | null = null;
  
  constructor(private courseService: CourseService, private router: Router) { }
  
  ngOnInit(): void { 
    // this.courseId = 0;
    this.getSubjects();
  }


  getSubjects(){
    this.courseService.getSubjects().subscribe(subject => { // Subscribe to the observable here
      this.subject = subject;
      console.log(this.subject);
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

  deleteClick(id:number){
    this.subjectId = id;
  }

  deleteSubject(){
    $('#proceedModal').modal('hide');
    console.log(this.subjectId);
    if(this.subjectId == 0) return ;
     this.courseService.deleteSubject(this.subjectId).subscribe(
      subject => {
        this.successMessage = 'Deleted Successfully!';
        this.subjectId = 0;
      // Find the index of the deleted course in the array
      const index = this.subject.findIndex(c => c.id === this.subjectId);

      if (index !== -1) {
        // Remove the deleted course from the array
        this.subject.splice(index, 1);
      }
      },(error) => {
        this.errorMessage = 'Failed to delete';
        this.subjectId = 0;
      }
     )
  }


  modalHide(){
    $('#proceedModal').modal('hide');
  }



}
