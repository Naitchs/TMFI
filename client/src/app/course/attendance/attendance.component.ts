import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AttendanceDto } from 'src/app/_models/attendance-dto';
import { Course } from 'src/app/_models/course';
import { Profile } from 'src/app/_models/profile';
import { Subjects } from 'src/app/_models/subject';
import { CourseService } from 'src/app/_services/course.service';
declare var $: any;


@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.scss']
})
export class AttendanceComponent {

  subject: Subjects | undefined;
  course: Course | undefined;
  courseId: number | undefined;
  subjectId: number | undefined;
  studentsInCourse: Profile[] = [];
  markedAttendance: { [key: number]: string } = {};
  groupedAttendanceRecords: any[] = [];
  addAttendance: boolean = false;
  currentDate: Date = new Date();
  successMessage: string | null = null;
  errorMessage: string | null = null;
  attendanceId: number = 0;
  statusToEdit: string = '';

  constructor(private courseService: CourseService, private route: ActivatedRoute,
    private router: Router) { }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const encryptedCourseId = params['courseId'];
      const encryptedSubjectId = params['subjectId'];

      // Decrypt the id using base64 decoding
      if (encryptedCourseId && encryptedSubjectId) {
        this.courseId = +atob(encryptedCourseId);
        this.subjectId = +atob(encryptedSubjectId);

        this.loadSubject(this.subjectId);
        this.loadStudents(this.courseId);

        this.courseService.getGroupedAttendanceRecords(this.subjectId).subscribe(
          (data) => {
            // Convert the object to an array
            this.groupedAttendanceRecords = Object.keys(data).map((date) => ({
              date,
              attendanceRecords: data[date],
            }));

            console.log('Grouped Attendance Records:', this.groupedAttendanceRecords);
          },
          (error) => {
            this.errorMessage = 'Error fetching grouped attendance records:', error;
          }
        );
      }
    });

    this.currentDate = new Date();
    this.attendanceId = 0;

  }


  markAttendance(studentId: number, attendanceStatus: string) {
    // Store the attendance status for each student in the markedAttendance object
    this.markedAttendance[studentId] = attendanceStatus;
  }

  submitAttendance() {
    this.successMessage = null;
    this.errorMessage = null;
    // Prepare an array of attendance records to be submitted to the backend
    const attendanceRecords = Object.keys(this.markedAttendance).map((studentId) => {
      return {
        studentId: +studentId,
        subjectId: this.subject?.id || 0, // Adjust accordingly based on your logic
        status: this.markedAttendance[+studentId],
      };
    });

    this.courseService.createMultipleAttendances(attendanceRecords).subscribe(
      () => {
        this.successMessage = 'Attendance records created successfully.';
        this.route.params.subscribe(params => {
          const encryptedCourseId = params['courseId'];
          const encryptedSubjectId = params['subjectId'];

          // Decrypt the id using base64 decoding
          if (encryptedCourseId && encryptedSubjectId) {
            this.courseId = +atob(encryptedCourseId);
            this.subjectId = +atob(encryptedSubjectId);

            this.loadSubject(this.subjectId);
            this.loadStudents(this.courseId);
            this.loadCourse(this.courseId);

            this.courseService.getGroupedAttendanceRecords(this.subjectId).subscribe(
              (data) => {
                // Convert the object to an array
                this.groupedAttendanceRecords = Object.keys(data).map((date) => ({
                  date,
                  attendanceRecords: data[date],
                }));

                // console.log('Grouped Attendance Records:', this.groupedAttendanceRecords);
              },
              (error) => {
                this.errorMessage = 'Error fetching grouped attendance records:', error;
              }
            );
          }
        });
      },
      (error) => {
        this.errorMessage = 'Error creating attendance records: ', error;
      }
    );
  }

  getFormattedDate(dateOfBirth: string): string {
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateOfBirth).toLocaleDateString(undefined, options);
  }

  // markAttendance(studentId: number, attendanceStatus: string) {
  //   const attendanceDto: AttendanceDto = {
  //     studentId: studentId,
  //     subjectId: this.subject.id, // Assuming this.subject contains the subject details; adjust accordingly
  //     status: attendanceStatus,
  //   };

  //   console.log(studentId);

  //   this.courseService.createAttendance(attendanceDto).subscribe(
  //     () => {
  //       // Handle success
  //       console.log('Attendance record created successfully.');
  //       // Optionally, perform additional actions after a successful creation
  //     },
  //     (error) => {
  //       // Handle error
  //       console.error('Error creating attendance record:', error);
  //       // Optionally, display an error message to the user or perform other actions
  //     }
  //   );
  // }

  loadCourse(id: number) {
    this.courseService.getCourse(id).subscribe({
      next: course => {
        // console.log(ipProfile);
        this.course = course;
      }
    });
  }


  loadSubject(id: number) {
    this.courseService.getSubject(id).subscribe({
      next: subject => {
        this.subject = subject;
      }
    });
  }

  loadStudents(id: number) {
    this.courseService.getStudentsInCourse(id).subscribe({
      next: students => {
        this.studentsInCourse = students;
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

  showAddAttendance() {
    this.successMessage = null;
    this.errorMessage = null;
    this.addAttendance = true;
  }

  cancel() {
    this.successMessage = null;
    this.errorMessage = null;
    this.addAttendance = false;
  }


  isEditable(record: any): boolean {
    // Assuming record.date is a string in 'YYYY-MM-DD' format
    const recordDate = new Date(record.date);
    const currentDate = new Date();

    // Set the time components to midnight for both dates
    recordDate.setHours(0, 0, 0, 0);
    currentDate.setHours(0, 0, 0, 0);

    // Check if the record date is equal to the current date
    return recordDate.getTime() === currentDate.getTime();
  }

  clickEditAttendance(id: number) {
    this.attendanceId = id;
    this.successMessage = null;
    this.errorMessage = null;
    $('#editAttendanceModal').modal('show');
  }

  editAttendance() {
    $('#editAttendanceModal').modal('hide');
    this.successMessage = null;
    this.errorMessage = null;
      this.courseService.editAttendance(this.attendanceId, this.statusToEdit).subscribe(
        () => {
          this.successMessage = 'Attendance edited successfully.';
          this.route.params.subscribe(params => {
            const encryptedCourseId = params['courseId'];
            const encryptedSubjectId = params['subjectId'];
      
            // Decrypt the id using base64 decoding
            if (encryptedCourseId && encryptedSubjectId) {
              this.courseId = +atob(encryptedCourseId);
              this.subjectId = +atob(encryptedSubjectId);
      
              this.loadSubject(this.subjectId);
              this.loadStudents(this.courseId);
      
              this.courseService.getGroupedAttendanceRecords(this.subjectId).subscribe(
                (data) => {
                  // Convert the object to an array
                  this.groupedAttendanceRecords = Object.keys(data).map((date) => ({
                    date,
                    attendanceRecords: data[date],
                  }));

                },
                (error) => {
                  this.errorMessage = 'Error fetching grouped attendance records:', error;
                }
              );
            }
          });


          this.attendanceId = null;
          this.statusToEdit = '';

         
        },
        (error) => {
          this.errorMessage = 'Error editing attendance:';
        }
      );
  }

  redirectToDetail(id: number) {
    this.router.navigate(['/course-detail', id]);
  }


}
