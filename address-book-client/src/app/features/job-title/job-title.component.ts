import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { JobTitle } from '../../shared/models/ِAddress';
import { JobTitleService } from '../services/jobtitle.service';


@Component({
  selector: 'app-job-title',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './job-title.component.html',
  styleUrl: './job-title.component.css'
})
export class JobTitleComponent implements OnInit {

  jobTitles: JobTitle[] = [];

  jobForm!: FormGroup;

  showModal = false;
  editingId: number | null = null;

  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private jobTitleService: JobTitleService
  ) {}

  ngOnInit(): void {

    this.jobForm = this.fb.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(150)
        ]
      ]
    });

    this.loadJobTitles();
  }

  loadJobTitles(): void {

    this.loading = true;
    this.errorMessage = '';

    this.jobTitleService.getAll().subscribe({
      next: (data) => {
        this.jobTitles = data;
        this.loading = false;
      },

      error: (error) => {
        console.error(error);
        this.errorMessage = 'Failed to load job titles.';
        this.loading = false;
      }
    });
  }

  openAddModal(): void {

    this.editingId = null;

    this.jobForm.reset();

    this.showModal = true;
  }

  openEditModal(jobTitle: JobTitle): void {

    this.editingId = jobTitle.id;

    this.jobForm.patchValue({
      name: jobTitle.name
    });

    this.showModal = true;
  }

  closeModal(): void {

    this.showModal = false;
    this.editingId = null;

    this.jobForm.reset();
  }

  save(): void {

    if (this.jobForm.invalid) {
      this.jobForm.markAllAsTouched();
      return;
    }

    const name = this.jobForm.value.name;

    this.loading = true;
    this.errorMessage = '';

    // ADD
    if (this.editingId === null) {

      this.jobTitleService.create(name).subscribe({

        next: (jobTitle) => {

          this.jobTitles = [
            ...this.jobTitles,
            jobTitle
          ];

          this.loading = false;
          this.closeModal();
        },

        error: (error) => {

          console.error(error);

          this.errorMessage =
            error?.error?.message ||
            'Failed to create job title.';

          this.loading = false;
        }
      });

      return;
    }

    // UPDATE
    this.jobTitleService
      .update(this.editingId, name)
      .subscribe({

        next: () => {

          this.jobTitles = this.jobTitles.map(job =>
            job.id === this.editingId
              ? { ...job, name }
              : job
          );

          this.loading = false;
          this.closeModal();
        },

        error: (error) => {

          console.error(error);

          this.errorMessage =
            error?.error?.message ||
            'Failed to update job title.';

          this.loading = false;
        }
      });
  }

  delete(id: number): void {

    const confirmed = window.confirm(
      'Are you sure you want to delete this job title?'
    );

    if (!confirmed) {
      return;
    }

    this.jobTitleService.delete(id).subscribe({

      next: () => {

        this.jobTitles = this.jobTitles.filter(
          job => job.id !== id
        );
      },

      error: (error) => {

        console.error(error);

        this.errorMessage =
          error?.error?.message ||
          'Failed to delete job title.';
      }
    });
  }

  isInvalid(controlName: string): boolean {

    const control = this.jobForm.get(controlName);

    return !!(
      control &&
      control.invalid &&
      (control.touched || control.dirty)
    );
  }
}