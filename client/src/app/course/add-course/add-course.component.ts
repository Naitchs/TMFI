import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CourseService } from 'src/app/_services/course.service';

@Component({
  selector: 'app-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss']
})
export class AddCourseComponent {

  
  constructor(private course: CourseService,  private fb: FormBuilder) {
  }
  successMessage: string | null = null;
  errorMessage: string | null = null;
  courseForm: FormGroup = new FormGroup({});
  
 
  ngOnInit(): void {
    this.initializeForm();
  }


  initializeForm(){
    this.courseForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      durationInHours: ['', Validators.required],
      tags: [''],
    });
  }

  createCourse(){
    const values = {...this.courseForm.value};
    this.course.addCourse(values).subscribe(
      () => {
           this.successMessage = 'Course Added Successfully!';
           this.courseForm.reset();
      },(error) =>{
           this.errorMessage = 'Error! Please Try Again!';
      }
    )
  }


}
