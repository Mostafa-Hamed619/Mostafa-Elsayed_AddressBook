import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Department } from '../../shared/models/ِAddress';
import { DepartmentService } from '../services/department.service';



@Component({
  selector: 'app-department',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './department.component.html',
  styleUrl: './department.component.css'
})
export class DepartmentComponent implements OnInit {

  departments: Department[] = [];

  departmentForm!: FormGroup;

  showModal = false;
  editingId: number | null = null;

  loading = false;
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {

    this.departmentForm = this.fb.group({
      name: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(150)
        ]
      ]
    });

    this.loadDepartments();
  }

  loadDepartments(): void {

    this.loading = true;
    this.errorMessage = '';

    this.departmentService.getAll().subscribe({

      next: (data) => {

        this.departments = data;
        this.loading = false;
      },

      error: (error) => {

        console.error(error);

        this.errorMessage =
          'Failed to load departments.';

        this.loading = false;
      }
    });
  }

  openAddModal(): void {

    this.editingId = null;

    this.departmentForm.reset();

    this.showModal = true;
  }

  openEditModal(department: Department): void {

    this.editingId = department.id;

    this.departmentForm.patchValue({
      name: department.name
    });

    this.showModal = true;
  }

  closeModal(): void {

    this.showModal = false;
    this.editingId = null;

    this.departmentForm.reset();
  }

  save(): void {

    if (this.departmentForm.invalid) {

      this.departmentForm.markAllAsTouched();

      return;
    }

    const name = this.departmentForm.value.name;

    this.loading = true;
    this.errorMessage = '';

    if (this.editingId === null) {

      this.departmentService.create(name).subscribe({

        next: (department) => {

          this.departments = [
            ...this.departments,
            department
          ];

          this.loading = false;

          this.closeModal();
        },

        error: (error) => {

          console.error(error);

          this.errorMessage =
            error?.error?.message ||
            'Failed to create department.';

          this.loading = false;
        }
      });

      return;
    }

    this.departmentService
      .update(this.editingId, name)
      .subscribe({

        next: () => {

          this.departments =
            this.departments.map(department =>
              department.id === this.editingId
                ? { ...department, name }
                : department
            );

          this.loading = false;

          this.closeModal();
        },

        error: (error) => {

          console.error(error);

          this.errorMessage =
            error?.error?.message ||
            'Failed to update department.';

          this.loading = false;
        }
      });
  }

  delete(id: number): void {

    const confirmed = window.confirm(
      'Are you sure you want to delete this department?'
    );

    if (!confirmed) {
      return;
    }

    this.departmentService.delete(id).subscribe({

      next: () => {

        this.departments =
          this.departments.filter(
            department => department.id !== id
          );
      },

      error: (error) => {

        console.error(error);

        this.errorMessage =
          error?.error?.message ||
          'Failed to delete department.';
      }
    });
  }

  isInvalid(controlName: string): boolean {

    const control =
      this.departmentForm.get(controlName);

    return !!(
      control &&
      control.invalid &&
      (control.touched || control.dirty)
    );
  }
}