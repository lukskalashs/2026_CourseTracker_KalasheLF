import { Routes } from '@angular/router';
import { Home } from '../Features/home/home';
import { Contact } from '../Features/contact/contact';
import { Disclaimer } from '../Features/disclaimer/disclaimer';
import { CourseList } from '../Features/courses/course-list/course-list';
import { Register } from '../Features/account/register/register';
import { CourseAdd } from '../Features/courses/course-add/course-add';
import { CourseDetails } from '../Features/courses/course-details/course-details';
import { courseResolver } from '../Features/courses/course-resolver';
import { CourseEdit } from '../Features/courses/course-edit/course-edit';
import { authGuard } from '../core/guards/auth.guard';

export const routes: Routes = [
    {path: '', component: Home},
      {path: 'register', component: Register},
    {path: 'contact', component: Contact},
    {path: 'disclaimer',component: Disclaimer},
    {path: 'courses', component: CourseList, canActivate: [authGuard]},
     
    {path: 'courses/add', component: CourseAdd, canActivate: [authGuard]},
    {path: 'courses/:id', component: CourseDetails, 
        resolve: {course: courseResolver},
        canActivate: [authGuard]},
    {path: 'courses/edit/:id', component: CourseEdit,
        resolve: {course: courseResolver},
        canActivate: [authGuard]
    },
    {path: '**', redirectTo: '', pathMatch: 'full'}
];
