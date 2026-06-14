import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  // standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './nav.html',
  styleUrls: ['./nav.css'],
})
export class Nav {
  accountService = inject(AccountService)
  private router = inject(Router)
  private toastService = inject(ToastService)

  model: any = {};

  login(){
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.toastService.show('Login successful!', 'success');
        this.router.navigateByUrl('/courses');
        this.model = {} // make sure the form is cleared
      },
      error: (err) => {
        const errorMessage = this.LoginWrrorMesaage(err);
        this.toastService.show(errorMessage, 'error');
      }
    })
  }

    LoginWrrorMesaage(error: any): string {
    if (!error || !error.error) {
      return 'Login failed. Please try again.';
    }
    
    if (typeof error.error === 'string') {
      return error.error;
    }
    
    return error.error.message || 'Login failed. Please try again.';
  }

  logout(){
    this.accountService.logout().subscribe({
      next: () => {
        this.toastService.show('Logged out Successfuly', 'success');
        this.router.navigateByUrl('/')
      }
    })
  }


}
