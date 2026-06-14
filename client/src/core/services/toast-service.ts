import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  toasts = signal<Toast[]>([]);



  show(message: string, type: 'success'|'error'|'info' = 'info'){
    const toast = { message, type };
    this.toasts.update(t => [...t, toast]);

    //3 sec timer
    setTimeout(() => this.remove(toast), 3000)
  }

  remove(toast: Toast) {
    this.toasts.update(t => t.filter(x => x !== toast));
  }
}

export interface Toast {
  message: string;
  type: 'success' | 'error' | 'info';
}
