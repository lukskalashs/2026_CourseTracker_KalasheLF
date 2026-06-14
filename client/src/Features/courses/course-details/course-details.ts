import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CourseService } from '../../../core/services/course-service';
import { ToastService } from '../../../core/services/toast-service';
import { Course } from '../../../types/course';
import { toSignal } from '@angular/core/rxjs-interop';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-course-details',
  imports: [DatePipe, RouterLink],
  templateUrl: './course-details.html',
  styleUrl: './course-details.css',
})
export class CourseDetails implements OnInit {

  route = inject(ActivatedRoute)
  router = inject(Router);
  courseService = inject(CourseService);
  toastService = inject(ToastService)

  course!: Course

  ngOnInit(): void {
    this.route.data.subscribe({
      next: fetchedData => {
        this.course = fetchedData["course"]
      }
    })
  }
  
  DeleteCourse(){
    this.courseService.deleteCourse(this.course.id).subscribe({
      next: () => {
        this.toastService.show("Course is Deleted Successfully", "error");
        this.router.navigateByUrl('/courses')
      }
    })
  }
}
