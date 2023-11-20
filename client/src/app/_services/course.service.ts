import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Course } from '../_models/course';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, map, of } from 'rxjs';
import { Subjects } from '../_models/subject';
import { SubjectsCourse } from '../_models/subjects-course';
import { EnrollStudent } from '../_models/enroll-student';
import { Profile } from '../_models/profile';
import { AttendanceDto } from '../_models/attendance-dto';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  baseUrl = environment.apiUrl;
  course: Course[] = [];
  subject: Subjects[] = [];
  deletedCourse: any;
  deletedSubject: any;
  ip: Profile[] = [];

  constructor(private http: HttpClient) { }

  addCourse(model: any) {
    return this.http.post<Course>(this.baseUrl + 'course/add-course', model);
  }

  updateCourse(course: Course, id: number) {
    return this.http.put(this.baseUrl + 'course/edit-course/' + id, course).pipe(
      map(() => {
        const index = this.course.indexOf(course);
        this.course[index] = { ...this.course[index], ...course }
      })
    )
  }

  getCourse(id: number) {
    const course = this.course.find(x => x.id === id);
    if (course) return of(course);
    return this.http.get<Course>(this.baseUrl + 'course/get-course/' + id);
  }

  getCourses() {
    if (this.course.length > 0) return of(this.course);
    return this.http.get<Course[]>(this.baseUrl + 'course/get-courses').pipe(
      map(course => {
        this.course = course;
        return course;
      })
    )
  };

  deleteCourse(id: number) {
    return this.http.delete(this.baseUrl + 'course/delete-course/' + id).pipe(
      map(course => {
        this.deletedCourse = course;
        return course;
      })
    )
  };

  addSubject(model: any) {
    return this.http.post<Subjects>(this.baseUrl + 'course/add-subject', model);
  };

  getSubjects() {
    if (this.subject.length > 0) return of(this.subject);
    return this.http.get<Subjects[]>(this.baseUrl + 'course/get-subjects').pipe(
      map(subject => {
        this.subject = subject;
        return subject;
      })
    )
  };

  getSubject(id: number) {
    const subject = this.subject.find(x => x.id === id);
    if (subject) return of(subject);
    return this.http.get<Subjects>(this.baseUrl + 'course/get-subject/' + id);
  }

  deleteSubject(id: number) {
    return this.http.delete(this.baseUrl + 'course/delete-subject/' + id).pipe(
      map(subject => {
        this.deletedSubject = subject;
        return subject;
      })
    )
  };


  addSubjectsToCourse(dto: SubjectsCourse): Observable<any> {
    return this.http.post(this.baseUrl + 'course/add-subjects-to-course', dto);
  }

  getSubjectsInCourse(courseId: number): Observable<Subjects[]> {
    return this.http.get<Subjects[]>(this.baseUrl + 'course/get-subjects-in-course/' + courseId);
  }

  getSubjectsNotInCourse(courseId: number): Observable<Subjects[]> {
    return this.http.get<Subjects[]>(this.baseUrl + 'course/get-subjects-not-in-course/' + courseId);
  }

  removeSubjectsFromCourse(courseId: number, subjectId: number) {
    return this.http.delete(this.baseUrl + 'course/remove-subjects-from-course', { body: { CourseId: courseId, SubjectId: subjectId } });
  }

  addStudentToCourse(dto: EnrollStudent): Observable<any> {
    return this.http.post(this.baseUrl + 'course/add-student-to-course', dto);
  }

  searchStudentsNotInCourse(courseId: number, search: string | null): Observable<any[]> {
    const url = `${this.baseUrl}course/search-students-not-in-course?courseId=${courseId}&search=${search || ''}`;

    return this.http.get<any[]>(url)
      .pipe(
        // catchError(this.handleError)  // Add error handling if needed
        map(ip => {
          this.ip = ip;
          return ip;
        })
      )

  }

  private handleError(error: any) {
    console.error('API Error:', error);
    throw error;  // You can handle the error based on your application requirements
  }

  getStudentsNotInCourse(courseId: number): Observable<Profile[]> {
    return this.http.get<Profile[]>(this.baseUrl + 'course/get-students-not-in-course/' + courseId);
  }

  getStudentsInCourse(courseId: number): Observable<any> {
    return this.http.get(this.baseUrl + 'course/get-students-in-course/' + courseId);
  }

  removeStudentFromCourse(courseId: number, studentId: number) {
    return this.http.delete(this.baseUrl + 'course/remove-student-from-course', { body: { courseId: courseId, studentId: studentId } });
  }


  createAttendance(attendanceDto: AttendanceDto): Observable<any> {
    return this.http.post(this.baseUrl + 'course/create-attendance', attendanceDto);
  }

  createMultipleAttendances(attendanceRecords: AttendanceDto[]): Observable<any> {
    return this.http.post(this.baseUrl + 'course/create-multiple-attendances', attendanceRecords);
  }

  getGroupedAttendanceRecords(subjectId: number): Observable<any> {
    return this.http.get(this.baseUrl + 'course/get-grouped-attendance-records/' + subjectId);
  }

  editAttendance(attendanceId: number, newStatus: string): Observable<any> {
    const headers = { 'Content-Type': 'application/json' };
    const body = { status: newStatus }; // Wrap newStatus in an object with a property named 'status'
    return this.http.put(this.baseUrl + 'course/edit-attendance/' + attendanceId, body, { headers });
  }
  
  
  
  



}
