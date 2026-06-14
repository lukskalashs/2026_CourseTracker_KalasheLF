import { Component, inject, OnInit } from '@angular/core';
import { CourseService } from '../../../core/services/course-service';
import { ToastService } from '../../../core/services/toast-service';
import { Course } from '../../../types/course';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-course-list',
  // standalone: true,
  imports: [DatePipe, RouterModule, CommonModule],
  templateUrl: './course-list.html',
  styleUrls: ['./course-list.css'],
})
export class CourseList implements OnInit {

   courseService = inject(CourseService)
   toastService = inject(ToastService)

   allCourses: Course[] = [];
   displayedCourses: Course[] = [];
   rCount = 0;

   //Pagination

   currentPage = 1;
   pageSize = 10;
   totalPages = 1;

  ngOnInit(): void {
    this.LoadAllCourses();
    this.LoadTheRecentCount();
  }


  LoadTheRecentCount(){
    this.courseService.getRecentCourses().subscribe({
      next: (courses) => {
        this.rCount = courses.length;
      }
    })
  }
  LoadAllCourses(){
    this.courseService.getCourses().subscribe({
      next: (courses) => {
        this.allCourses = courses;
        this.totalPages = Math.max(1, Math.ceil(this.allCourses.length / this.pageSize));
        this.updatePagination();
      }
    })
  }
  updatePagination(){
    const indexStart = (this.currentPage -1) * this.pageSize;
    const indexEnd = indexStart + this.pageSize;
    this.displayedCourses = this.allCourses.slice(indexStart, indexEnd);
  }

  NextPage(){
    if(this.currentPage < this.totalPages){
      this.currentPage += 1;
      this.updatePagination();
    }
  }
  PrevPage(){
    if(this.currentPage > 1)
    {
      this.currentPage -= 1;
      this.updatePagination();
    }
  }

  //Use patch api to toggle completion

  MarkComplete(id: number){
    this.courseService.markAsCompleted(id).subscribe({
      next: () => {
        this.toastService.show('Course marked as completed', 'success');
        const course = this.allCourses.find(c => c.id === id);
        if (course) {
          course.isCompleted = true;
          course.date = new Date().toISOString();
          this.updatePagination();
        }
        this.LoadTheRecentCount();
      },
      error: error => {
        console.error('Failed to mark course complete', error);
      }
    })
  }

  trackByCourseId(index: number, course: Course) {
    return course.id;
  }
} 
