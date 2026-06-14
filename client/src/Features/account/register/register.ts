import { Component, inject } from '@angular/core';
import { AccountService } from '../../../core/services/account-service';
import { ToastService } from '../../../core/services/toast-service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  accountService = inject(AccountService);
  toastService = inject(ToastService)
  private router = inject(Router);


  Regmodel: any = {};

    

  Register(){
    this.accountService.register(this.Regmodel).subscribe({
    next: () => {
        this.toastService.show("The Registration was successfull!", "success");
        this.router.navigateByUrl("/courses");
      },
      error: (err) => {
        const msg = this.RegErrorsFormar(err);
        this.toastService.show(msg, 'error');
        console.error('Registration error', err);
      }
    })
  }

  cancelReg(){
    this.router.navigateByUrl('/')
  }

  RegErrorsFormar(errors: any): string {
    if (!errors) return 'Registration has failed';

    if (typeof errors === 'string') return errors;

    if (errors.error) {
      if (typeof errors.error === 'string') return errors.error;
      if (Array.isArray(errors.error)) {
        return errors.error
          .map((err: any) => err.description || err.message || JSON.stringify(err))
          .join(', ');
      }
      if (typeof errors.error === 'object') {
        if (Array.isArray(errors.error.errors)) {
          return errors.error.errors
            .map((e: any) => e.description || e.message || JSON.stringify(e))
            .join(', ');
        }
        return errors.error.description || errors.error.message || JSON.stringify(errors.error);
      }
    }

    return errors.message || 'Registration failed';
  }
}
