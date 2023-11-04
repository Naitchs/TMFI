import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Course } from '../_models/course';
import { HttpClient } from '@angular/common/http';
import { map, of } from 'rxjs';
import { Subject } from '../_models/subject';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  baseUrl = environment.apiUrl;
  course: Course [] = [];
  subject: Subject [] = [];
  deletedCourse: any;
  deletedSubject: any;

  constructor(private http: HttpClient) { }

  addCourse(model: any){
    return this.http.post<Course>(this.baseUrl + 'course/add-course', model);
  }

  updateCourse(course: Course, id: number){
    return this.http.put(this.baseUrl + 'course/edit-course/' + id, course).pipe(
      map(() => {
        const index = this.course.indexOf(course);
        this.course[index] = {...this.course[index], ...course}
      })
    )
  }

  getCourse(id: number){
    const course = this.course.find(x => x.id === id);
    if (course) return of (course);
    return this.http.get<Course>(this.baseUrl + 'course/get-course/' + id);
  }

  getCourses(){
    if (this.course.length > 0) return of (this.course);
    return this.http.get<Course[]>(this.baseUrl + 'course/get-courses').pipe(
      map(course => {
        this.course = course;
        return course;
      })
    )
  };

  deleteCourse(id: number){
    return this.http.delete(this.baseUrl + 'course/delete-course/' + id).pipe(
      map(course => {
        this.deletedCourse = course;
        return course;
      })
    )
  };

  addSubject(model: any){
    return this.http.post<Subject>(this.baseUrl + 'course/add-subject', model);
  };

  getSubjects(){
    if (this.subject.length > 0) return of (this.subject);
    return this.http.get<Subject[]>(this.baseUrl + 'course/get-subjects').pipe(
      map(subject => {
        this.subject = subject;
        return subject;
      })
    )
  };

  deleteSubject(id: number){
    return this.http.delete(this.baseUrl + 'course/delete-subject/' + id).pipe(
      map(subject => {
        this.deletedSubject = subject;
        return subject;
      })
    )
  };

}
