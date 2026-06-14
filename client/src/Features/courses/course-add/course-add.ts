import { Component, Inject, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../../../core/services/course-service';
import { ToastService } from '../../../core/services/toast-service';
import { Router } from '@angular/router';
import { Platform, Topic } from '../../../types/lookup';

@Component({
  selector: 'app-course-add',
  imports: [FormsModule],
  templateUrl: './course-add.html',
  styleUrl: './course-add.css',
})
export class CourseAdd implements OnInit{

  courseService = inject(CourseService)
  toastService = inject(ToastService)
  private router = inject(Router)

  AddCourseModel: any = { isCompleted: false, platformId: null, topicId: null }
  Platforms: Platform[] = [];
  Topis: Topic[] = [];

  ngOnInit(): void {
   this.LookupLoad()
  }

  cancel(){
    this.router.navigateByUrl("/courses")
  }
  addCourse(){
    if (this.AddCourseModel.platformId !== null && this.AddCourseModel.platformId !== undefined) {
      this.AddCourseModel.platformId = Number(this.AddCourseModel.platformId);
    }
    if (this.AddCourseModel.topicId !== null && this.AddCourseModel.topicId !== undefined) {
      this.AddCourseModel.topicId = Number(this.AddCourseModel.topicId);
    }

    console.log('AddCourseModel before submit', this.AddCourseModel);

    this.courseService.addCourse(this.AddCourseModel).subscribe({
      next: () => {
        this.toastService.show("Course was Added Successfully", "success");
        this.router.navigateByUrl('/courses');
      },
      error: (err) => {
        console.error('Add course failed', err);
        this.toastService.show('Failed to add course: ' + (err?.error || err?.message || 'Unknown'), 'error');
      }
    });
  }
  LookupLoad(){
    this.courseService.getPlatforms().subscribe({
      next: plat => this.Platforms = plat
    })
    this.courseService.getTopics().subscribe({
      next: tpcs => this.Topis = tpcs
    })
  }
  
  
}
