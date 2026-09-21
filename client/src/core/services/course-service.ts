import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { User } from '../../types/user';
import { Course } from '../../types/course';
import { retry } from 'rxjs';
import { Platform, Topic } from '../../types/lookup';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  currentUser = signal<User | null>(null);

  getCourses(){
    return this.http.get<Course[]>(this.baseUrl + 'courses');
  }

  getRecentCourses(){
    return this.http.get<Course[]>(this.baseUrl + 'courses/pastmonth');
  }
  getCourse(id: number){
    return this.http.get<Course>(this.baseUrl + 'courses/' + id)
    
  }

  addCourse(course: Course | any)
  {
    return this.http.post<Course>(this.baseUrl + 'courses', course);
  }

  updateCourse(id: number, course: Course | any)
  {
    return this.http.put(this.baseUrl + 'courses/' + id, course);
  }
  markAsCompleted(id: number){
    return this.http.patch(this.baseUrl + 'courses/' + id, {});
  }
  deleteCourse(id: number){
    return this.http.delete(this.baseUrl + 'courses/' + id);

  }

  getPlatforms(){
    return this.http.get<Platform[]>(this.baseUrl + 'platforms');
  }
  getTopics(){
    return this.http.get<Topic[]>(this.baseUrl + 'topics');
  }


  
}
