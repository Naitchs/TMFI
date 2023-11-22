import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) =>{
        if (error) {
          switch (error.status){
            // case 200: // Handle successful response (not an error)
            // const message = error.error; // Assuming the message is in error.error
            // this.toastr.success(message, 'Success');
            // break;

            // case 400: 
            //   if (error.error.errors){
            //     const modalStateErrors = [];
            //     for (const key in error.error.errors){
            //       if (error.error.errors [key]){
            //         modalStateErrors.push(error.error.errors[key])
            //       }
            //     }
            //     throw modalStateErrors.flat();
            //   } else {
            //     this.toastr.error(error.error, error.status.toString())
            //   }
            //   break;
              case 401:
                this.toastr.error('Unathorized', error.status.toString())
                // this.router.navigateByUrl('/log-in');
                break;
              case 404:
                this.router.navigateByUrl('/not-found');
                break;
              case 500:
                const navigationExtras: NavigationExtras = {state: {error: error.error}};
                this.router.navigateByUrl('/server-error', navigationExtras);
                break;
              // default:
              //   this.toastr.error('Something unexpected went wrong ');
              //   console.log(error);

              // break;
          }
        }
        throw error;
      })
    )
  }
}
