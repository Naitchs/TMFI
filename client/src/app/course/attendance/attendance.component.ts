import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/_models/course';
import { Profile } from 'src/app/_models/profile';
import { PhaseEnum, Subjects } from 'src/app/_models/subject';
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
  canCreateAttendance: boolean = false;
  currentDate: Date = new Date();
  successMessage: string | null = null;
  errorMessage: string | null = null;
  attendanceId: number = 0;
  attendanceIdToDelete: number = 0;
  statusToEdit: string = '';
  selectedAttendanceIds: number[] = [];
  selectAll: boolean = true;
  show: boolean = false;
  validationError: boolean = false;
  phases: string[] = Object.keys(PhaseEnum).filter(key => isNaN(Number(PhaseEnum[key])));

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

            const today = new Date();
            today.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
            const hasAttendanceForToday = this.groupedAttendanceRecords.some(record => {
              const recordDate = new Date(record.date);
              recordDate.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
              return recordDate.getTime() === today.getTime();
            });

            this.canCreateAttendance = !hasAttendanceForToday;

            // this.selectedAttendanceIds = this.groupedAttendanceRecords
            //   .flatMap(record => record.attendanceRecords)
            //   .map(attendance => attendance.attendanceID);

            this.selectedAttendanceIds = this.groupedAttendanceRecords
              .flatMap(record => record.attendanceRecords)
              .filter(attendance => {
                const attendanceDate = new Date(attendance.date);
                attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
                return attendanceDate.getTime() === today.getTime();
              })
              .map(attendance => attendance.attendanceID);

            console.log('selecetded', this.selectedAttendanceIds);


            console.log('Grouped Attendance Records:', this.groupedAttendanceRecords);
          },
          (error) => {
            this.errorMessage = 'Error fetching grouped attendance records:', error;
          }
        );

        this.courseService.getCourse(this.courseId).subscribe({
          next: course => {
            console.log('Course data:', course);
            this.course = course;
          },
          error: err => {
            console.error('Error fetching course:', err);
          }
        });
      }
    });

    this.currentDate = new Date();
    this.attendanceId = 0;
    this.attendanceIdToDelete = 0;

  }

  toggleSelectAll(): void {
    this.selectedAttendanceIds = this.selectAll
      ? this.groupedAttendanceRecords
        .flatMap(record => record.attendanceRecords)
        .map(attendance => attendance.attendanceID)
      : [];
  }


  markAttendance(studentId: number, attendanceStatus: string) {
    // Store the attendance status for each student in the markedAttendance object
    this.markedAttendance[studentId] = attendanceStatus;
  }

  submitAttendance() {
    this.successMessage = null;
    this.errorMessage = null;

    // Check if attendance is selected for all students
    const allStudentsHaveAttendance = this.studentsInCourse.every(student => {
      const status = this.markedAttendance[student.id];
      return status !== undefined;
    });

    if (allStudentsHaveAttendance) {
      this.validationError = false;

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



              this.courseService.getGroupedAttendanceRecords(this.subjectId).subscribe(
                (data) => {
                  // Convert the object to an array
                  this.groupedAttendanceRecords = Object.keys(data).map((date) => ({
                    date,
                    attendanceRecords: data[date],
                  }));

                  const today = new Date();
                  today.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
                  const hasAttendanceForToday = this.groupedAttendanceRecords.some(record => {
                    const recordDate = new Date(record.date);
                    recordDate.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
                    return recordDate.getTime() === today.getTime();
                  });

                  this.canCreateAttendance = !hasAttendanceForToday;

                  this.selectedAttendanceIds = this.groupedAttendanceRecords
                    .flatMap(record => record.attendanceRecords)
                    .filter(attendance => {
                      const attendanceDate = new Date(attendance.date);
                      attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
                      return attendanceDate.getTime() === today.getTime();
                    })
                    .map(attendance => attendance.attendanceID);

                  console.log('selecetded', this.selectedAttendanceIds);


                  console.log('Grouped Attendance Records:', this.groupedAttendanceRecords);
                },
                (error) => {
                  this.errorMessage = 'Error fetching grouped attendance records:', error;
                }
              );

              this.courseService.getCourse(this.courseId).subscribe({
                next: course => {
                  console.log('Course data:', course);
                  this.course = course;
                },
                error: err => {
                  console.error('Error fetching course:', err);
                }
              });
            }
          });
          this.addAttendance = false;
        },
        (error) => {
          this.errorMessage = 'Error creating attendance records: ', error;
        }
      );
    } else {
      this.validationError = true;
    }
  }

  getFormattedDate(dateOfBirth: string): string {
    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(dateOfBirth).toLocaleDateString(undefined, options);
  }

  loadCourse(id: number) {
    this.courseService.getCourse(id).subscribe({
      next: course => {
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

            const today = new Date();
            today.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
            const hasAttendanceForToday = this.groupedAttendanceRecords.some(record => {
              const recordDate = new Date(record.date);
              recordDate.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
              return recordDate.getTime() === today.getTime();
            });

            this.canCreateAttendance = !hasAttendanceForToday;

            this.selectedAttendanceIds = this.groupedAttendanceRecords
              .flatMap(record => record.attendanceRecords)
              .filter(attendance => {
                const attendanceDate = new Date(attendance.date);
                attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
                return attendanceDate.getTime() === today.getTime();
              })
              .map(attendance => attendance.attendanceID);

          },
          (error) => {
            this.errorMessage = 'Error fetching grouped attendance records:', error;
          }
        );

        this.courseService.getCourse(this.courseId).subscribe({
          next: course => {
            console.log('Course data:', course);
            this.course = course;
          },
          error: err => {
            console.error('Error fetching course:', err);
          }
        });
      }
    });
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

  clickEditAttendance(id: number, status: string) {
    this.attendanceId = id;
    this.statusToEdit = status;
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

                const today = new Date();
                today.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
                const hasAttendanceForToday = this.groupedAttendanceRecords.some(record => {
                  const recordDate = new Date(record.date);
                  recordDate.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
                  return recordDate.getTime() === today.getTime();
                });

                this.canCreateAttendance = !hasAttendanceForToday;

                this.selectedAttendanceIds = this.groupedAttendanceRecords
                  .flatMap(record => record.attendanceRecords)
                  .filter(attendance => {
                    const attendanceDate = new Date(attendance.date);
                    attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
                    return attendanceDate.getTime() === today.getTime();
                  })
                  .map(attendance => attendance.attendanceID);

              },
              (error) => {
                this.errorMessage = 'Error fetching grouped attendance records:', error;
              }
            );

            this.courseService.getCourse(this.courseId).subscribe({
              next: course => {
                console.log('Course data:', course);
                this.course = course;
              },
              error: err => {
                console.error('Error fetching course:', err);
              }
            });
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
    const encryptedId = btoa(id.toString());
    this.router.navigate(['/course-detail', encryptedId]);
  }

  clickToDelete(id: number) {
    this.attendanceIdToDelete = id;
    this.successMessage = null;
    this.errorMessage = null;
    $('#deleteAttendanceModal').modal('show');
  }

  getPhaseName(phase: PhaseEnum): string {
    return PhaseEnum[phase];
  }

  // deleteAttendance() {
  //   $('#deleteAttendanceModal').modal('hide');
  //   console.log('delete', this.attendanceIdToDelete);
  //   this.courseService.deleteAttendace(this.attendanceIdToDelete).subscribe(
  //     () => {
  //       this.successMessage = 'Attendance Deleted successfully.';
  //       this.route.params.subscribe(params => {
  //         const encryptedCourseId = params['courseId'];
  //         const encryptedSubjectId = params['subjectId'];

  //         // Decrypt the id using base64 decoding
  //         if (encryptedCourseId && encryptedSubjectId) {
  //           this.courseId = +atob(encryptedCourseId);
  //           this.subjectId = +atob(encryptedSubjectId);

  //           this.loadSubject(this.subjectId);
  //           this.loadStudents(this.courseId);



  //           this.courseService.getGroupedAttendanceRecords(this.subjectId).subscribe(
  //             (data) => {
  //               // Convert the object to an array
  //               this.groupedAttendanceRecords = Object.keys(data).map((date) => ({
  //                 date,
  //                 attendanceRecords: data[date],
  //               }));

  //               const today = new Date();
  //               today.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
  //               const hasAttendanceForToday = this.groupedAttendanceRecords.some(record => {
  //                 const recordDate = new Date(record.date);
  //                 recordDate.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
  //                 return recordDate.getTime() === today.getTime();
  //               });

  //               this.canCreateAttendance = !hasAttendanceForToday;

  //               // this.selectedAttendanceIds = this.groupedAttendanceRecords
  //               //   .flatMap(record => record.attendanceRecords)
  //               //   .map(attendance => attendance.attendanceID);

  //               this.selectedAttendanceIds = this.groupedAttendanceRecords
  //               .flatMap(record => record.attendanceRecords)
  //               .filter(attendance => {
  //                 const attendanceDate = new Date(attendance.date);
  //                 attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
  //                 return attendanceDate.getTime() === today.getTime();
  //               })
  //               .map(attendance => attendance.attendanceID);

  //               console.log('selecetded', this.selectedAttendanceIds);


  //               console.log('Grouped Attendance Records:', this.groupedAttendanceRecords);
  //             },
  //             (error) => {
  //               this.errorMessage = 'Error fetching grouped attendance records:', error;
  //             }
  //           );

  //           this.courseService.getCourse(this.courseId).subscribe({
  //             next: course => {
  //               console.log('Course data:', course);
  //               this.course = course;
  //             },
  //             error: err => {
  //               console.error('Error fetching course:', err);
  //             }
  //           });
  //         }
  //       });

  //     }, (error) => {
  //       this.errorMessage = 'Error Deleting Attendance: ', error;
  //     }
  //   )
  // }


  isSelected(attendanceId: number): boolean {
    const attendance = this.findAttendanceById(attendanceId);

    if (attendance) {
      const attendanceDate = new Date(attendance.date);
      attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
      const today = new Date();
      today.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison

      return attendanceDate.getTime() === today.getTime();
    }

    return false;
  }

  toggleSelection(attendanceId: number): void {
    if (this.isSelected(attendanceId)) {
      this.selectedAttendanceIds = this.selectedAttendanceIds.filter(id => id !== attendanceId);
    } else {
      this.selectedAttendanceIds.push(attendanceId);
    }
  }

  clickToDeleteSelected(): void {
    $('#deleteAttendanceModal').modal('hide');
    const today = new Date();
    today.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison

    for (const attendanceId of this.selectedAttendanceIds) {
      const attendance = this.findAttendanceById(attendanceId);

      if (attendance) {
        const attendanceDate = new Date(attendance.date);
        attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison

        // Check if the attendance record date is today
        if (attendanceDate.getTime() === today.getTime()) {
          this.courseService.deleteAttendace(attendanceId).subscribe(
            () => {
              this.successMessage = 'Attendance Deleted successfully.';
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

                      const today = new Date();
                      today.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
                      const hasAttendanceForToday = this.groupedAttendanceRecords.some(record => {
                        const recordDate = new Date(record.date);
                        recordDate.setHours(0, 0, 0, 0);  // I-set ang oras sa midnight para i-ignore ang oras
                        return recordDate.getTime() === today.getTime();
                      });

                      this.canCreateAttendance = !hasAttendanceForToday;

                      this.selectedAttendanceIds = this.groupedAttendanceRecords
                        .flatMap(record => record.attendanceRecords)
                        .filter(attendance => {
                          const attendanceDate = new Date(attendance.date);
                          attendanceDate.setHours(0, 0, 0, 0);  // Set the time to midnight for comparison
                          return attendanceDate.getTime() === today.getTime();
                        })
                        .map(attendance => attendance.attendanceID);

                    },
                    (error) => {
                      this.errorMessage = 'Error fetching grouped attendance records:', error;
                    }
                  );

                  this.courseService.getCourse(this.courseId).subscribe({
                    next: course => {
                      console.log('Course data:', course);
                      this.course = course;
                    },
                    error: err => {
                      console.error('Error fetching course:', err);
                    }
                  });
                }
              });
            },
            (error) => {
              this.errorMessage = 'Error Deleting Attendance: ' + error;
            }
          );
        } else {
          this.errorMessage = 'Error to delete: Attendance date is not today.';
        }
      }
    }

  }


  private findAttendanceById(attendanceId: number): any {
    for (const record of this.groupedAttendanceRecords) {
      const attendance = record.attendanceRecords.find(a => a.attendanceID === attendanceId);
      if (attendance) {
        return attendance;
      }
    }
    return null;
  }





}
