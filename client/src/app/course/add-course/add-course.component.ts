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
  
  initializeForm(){
    this.courseForm = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      durationInHours: [''],
      tags: [''],
    });
  }

  createCourse(){
    // console.log(this.courseForm);
    this.course.addCourse(this.courseForm.value).subscribe(
      () => {
           console.log(this.courseForm);
           this.successMessage = 'Course Added Successfully!';
      },(error) =>{
           this.errorMessage = 'Error';
      }
    )
  }

  ngOnInit(): void {
    this.initializeForm();
  }


}
