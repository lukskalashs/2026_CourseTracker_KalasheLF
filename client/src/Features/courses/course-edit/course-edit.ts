import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../../../core/services/course-service';
import { ToastService } from '../../../core/services/toast-service';
import { Platform, Topic } from '../../../types/lookup';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-course-edit',
  imports: [FormsModule],
  templateUrl: './course-edit.html',
  styleUrl: './course-edit.css',
})
export class CourseEdit implements OnInit {

  route = inject(ActivatedRoute);
  router = inject(Router);
  courseService = inject(CourseService)
  toastService = inject(ToastService);

  CourseEditmodel: any = {}
  topics: Topic[] = [];
  platforms: Platform[] = [];
  CourseId!: number;

   ngOnInit(): void {
    this.LookupsLoad();

    this.route.data.subscribe({
      next: EditData => {
        const Course = EditData["course"];
        this.CourseId = Course.id;
        this.CourseEditmodel = {...Course} //The data is tehn inserted into thr form moddel {CourseEditmodel}
      }
    })
  }


  // lookup the topics and platform when editing 
  LookupsLoad(){
    this.courseService.getPlatforms().subscribe(
      pltfm => {
        this.platforms = pltfm
      }
    )
    this.courseService.getTopics().subscribe(
      tpcs => {
        this.topics = tpcs
      }
    )
  }

  UpdateCourse(){
    this.CourseEditmodel.pltId = Number(this.CourseEditmodel.pltId);
    this.CourseEditmodel.tpcId = Number(this.CourseEditmodel.tpcId);

    this.courseService.updateCourse(this.CourseId, this.CourseEditmodel).subscribe({
      next: () => {
        this.toastService.show('Course was updated Successfully');
        this.router.navigateByUrl("/courses/" + this.CourseId)
      }
    })
  }

  Cancel(){
    this.router.navigateByUrl('/courses/' + this.CourseId)
  }


 
}
