import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Router,
  RouterLink
} from '@angular/router';

import { AuthService } from '../../../core/service/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isLoading = false;

  errorMessage = '';

  successMessage = '';

  registerForm = this.fb.nonNullable.group({

    email: ['',[Validators.required,Validators.email,Validators.maxLength(256)]],

    password: ['',[Validators.required,Validators.minLength(6)]],
    confirmPassword: ['',[Validators.required]]

  });

  onSubmit(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }
    const {email,password,confirmPassword} = this.registerForm.getRawValue();
    if (password !== confirmPassword) {
      this.errorMessage ='Password and Confirm Password do not match.';

      return;
    }

    this.isLoading = true;

    this.authService.register({email,password}).subscribe({
        next: () => {
          this.isLoading = false;
          this.successMessage ='Registration completed successfully.';
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 1000);
        },
        error: error => {
          this.isLoading = false;
          this.errorMessage =
            error?.error?.message ??
            'Registration failed. Please try again.';
        }
      });
  }
}