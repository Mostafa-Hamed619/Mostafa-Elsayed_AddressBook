import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { AddressService } from '../../services/address.service';
import { Department, JobTitle } from '../../../shared/models/ِAddress';
import { JobTitleService } from '../../services/jobtitle.service';
import { DepartmentService } from '../../services/department.service';


@Component({
  selector: 'app-address-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './address-form.component.html',
  styleUrl: './address-form.component.css'
})
export class AddressFormComponent implements OnInit {

  addressForm!: FormGroup;

  jobTitles: JobTitle[] = [];
  departments: Department[] = [];

  selectedPhoto: File | null = null;

  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService,
    private jobTitleService: JobTitleService,
    private departmentService: DepartmentService,
    private router: Router
  ) {}

  ngOnInit(): void {

    this.addressForm = this.fb.group({

      fullName: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(150)
        ]
      ],

      jobId: [
        '',
        Validators.required
      ],

      departmentId: [
        '',
        Validators.required
      ],

      mobileNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^01[0125][0-9]{8}$/)
        ]
      ],

      dateOfBirth: [
        '',
        Validators.required
      ],

      addressLine: [
        '',
        [
          Validators.required,
          Validators.maxLength(500)
        ]
      ],

      email: [
        '',
        [
          Validators.required,
          Validators.email,
          Validators.maxLength(256)
        ]
      ],

      photo: [
        null
      ],

      age: [
        {
          value: '',
          disabled: true
        }
      ]
    });

    this.loadJobTitles();
    this.loadDepartments();
  }

  loadJobTitles(): void {

    this.jobTitleService.getAll().subscribe({
      next: (data) => {
        this.jobTitles = data;
      },

      error: (error) => {
        console.error(error);
        this.errorMessage = 'Failed to load job titles.';
      }
    });
  }

  loadDepartments(): void {

    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.departments = data;
      },

      error: (error) => {
        console.error(error);
        this.errorMessage = 'Failed to load departments.';
      }
    });
  }

  calculateAge(): void {

    const dateOfBirth = this.addressForm.get('dateOfBirth')?.value;

    if (!dateOfBirth) {
      this.addressForm.get('age')?.setValue('');
      return;
    }

    const birthDate = new Date(dateOfBirth);
    const today = new Date();

    let age = today.getFullYear() - birthDate.getFullYear();

    const monthDifference =
      today.getMonth() - birthDate.getMonth();

    if (
      monthDifference < 0 ||
      (
        monthDifference === 0 &&
        today.getDate() < birthDate.getDate()
      )
    ) {
      age--;
    }

    this.addressForm.get('age')?.setValue(age);
  }

  onPhotoSelected(event: Event): void {

    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) {
      this.selectedPhoto = null;
      return;
    }

    this.selectedPhoto = input.files[0];
  }

  submit(): void {

    if (this.addressForm.invalid) {

      this.addressForm.markAllAsTouched();

      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const formValue = this.addressForm.getRawValue();

    const formData = new FormData();

    formData.append('FullName', formValue.fullName);
    formData.append('JobId', formValue.jobId.toString());
    formData.append(
      'DepartmentId',
      formValue.departmentId.toString()
    );

    formData.append(
      'MobileNumber',
      formValue.mobileNumber
    );

    formData.append(
      'DateOfBirth',
      formValue.dateOfBirth
    );

    formData.append(
      'AddressLine',
      formValue.addressLine
    );

    formData.append(
      'Email',
      formValue.email
    );

    if (this.selectedPhoto) {
      formData.append(
        'Photo',
        this.selectedPhoto,
        this.selectedPhoto.name
      );
    }

    this.addressService.create(formData).subscribe({

      next: () => {

        this.loading = false;

        // No page reload
        this.router.navigate(['/address-book']);
      },

      error: (error) => {

        console.error(error);

        this.loading = false;

        this.errorMessage =
          error?.error?.message ||
          'Failed to create address.';
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/address-book']);
  }

  isInvalid(controlName: string): boolean {

    const control = this.addressForm.get(controlName);

    return !!(
      control &&
      control.invalid &&
      (control.touched || control.dirty)
    );
  }
}