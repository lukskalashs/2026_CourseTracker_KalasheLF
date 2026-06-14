import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BusyService } from '../core/services/busy-service';
import { ToastService } from '../core/services/toast-service';
import { AccountService } from '../core/services/account-service';
import { Nav } from '../Features/nav/nav';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Nav],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  
  busyService = inject(BusyService)
  toastService = inject(ToastService)
  accountService = inject(AccountService)

  ngOnInit(): void {
    this.setCurrentUser()
  }

  // in the local storage of the app we check if a user is already logged in

  setCurrentUser(){
    const userString = localStorage.getItem('user')
    if(!userString)
    {
      return;
    }
    const user = JSON.parse(userString)
    this.accountService.setCurrentUser(user)
  }

  
  
  // private http = inject(HttpClient)
  // protected readonly title = "Course Tracker";


  // ngOnInit(): void {
  //   this.http.get('https://localhost:5101/api/courses').subscribe({
  //     next: response => console.log(response),
  //     error: error => console.log(error),
  //     complete: () => console.log('Complete http request')
      
      
  //   })
  // }
}
