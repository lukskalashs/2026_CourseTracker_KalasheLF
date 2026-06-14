import { ResolveFn } from '@angular/router';
import { Course } from '../../types/course';
import { CourseService } from '../../core/services/course-service';
import { inject } from '@angular/core';

export const courseResolver: ResolveFn<Course> = (route, state) => {
  const courseService = inject(CourseService)

  //extract id from the url route
  const id = Number(route.paramMap.get('id'));
  return courseService.getCourse(id)
};
