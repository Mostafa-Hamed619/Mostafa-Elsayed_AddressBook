import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddressService } from '../../services/address.service';
import { Address, AddressSearch, Department, JobTitle } from '../../../shared/models/ِAddress';
import { AuthService } from '../../../core/service/auth.service';
import { Router } from '@angular/router';
import { JobTitleService } from '../../services/jobtitle.service';
import { DepartmentService } from '../../services/department.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-address-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './address-list.component.html',
  styleUrl: './address-list.component.css'
})
export class AddressList implements OnInit {

  addresses: Address[] = [];

  loading = false;
  errorMessage = '';
  jobTitles: JobTitle[] = [];
departments: Department[] = [];

  selectedPhoto: File | null = null;

  selectedAddress: Address | null = null;
  searchTerm = '';

dateOfBirthFrom = '';
dateOfBirthTo = '';

  showEditModal = false;
  constructor(
    private addressService: AddressService,
  private authService: AuthService,
  private router: Router,
  private jobTitleService: JobTitleService,
  private departmentService: DepartmentService
  ) {}

  ngOnInit(): void {
    this.loadAddresses();
  this.loadJobTitles();
  this.loadDepartments();
  }

  readonly apiBaseUrl = 'https://localhost:7107';

getPhotoUrl(photo: string | null | undefined): string {
  if (!photo) {
    return '';
  }

  return `${this.apiBaseUrl}${photo}`;
}

editAddress(address: Address): void {
  this.selectedAddress = address;
  this.showEditModal = true;
  this.selectedPhoto = null;
}

closeEditModal(): void {
  this.showEditModal = false;
  this.selectedAddress = null;
  this.selectedPhoto = null;
}

recalculateAge(): void {
  if (!this.selectedAddress) return;

  const dob = new Date(this.selectedAddress.dateOfBirth);
  const today = new Date();

  let age = today.getFullYear() - dob.getFullYear();

  const m = today.getMonth() - dob.getMonth();
  if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
    age--;
  }

  this.selectedAddress.age = age;
}

onPhotoSelected(event: Event): void {

  const input = event.target as HTMLInputElement;

  if (input.files && input.files.length > 0) {
    this.selectedPhoto = input.files[0];
  }
}

updateAddress(): void {

  if (!this.selectedAddress) {
    return;
  }

  const formData = new FormData();

  formData.append(
    'FullName',
    this.selectedAddress.fullName
  );

  formData.append(
    'JobId',
    this.selectedAddress.jobId.toString()
  );

  formData.append(
    'DepartmentId',
    this.selectedAddress.departmentId.toString()
  );

  formData.append(
    'MobileNumber',
    this.selectedAddress.mobileNumber
  );

  formData.append(
    'DateOfBirth',
    this.selectedAddress.dateOfBirth
  );

  formData.append(
    'AddressLine',
    this.selectedAddress.addressLine
  );

  formData.append(
    'Email',
    this.selectedAddress.email
  );

  if (this.selectedPhoto) {
    formData.append(
      'Photo',
      this.selectedPhoto
    );
  }

  this.addressService
    .update(this.selectedAddress.id, formData)
    .subscribe({

      next: () => {

        this.showEditModal = false;
        this.selectedAddress = null;
        this.selectedPhoto = null;

        this.loadAddresses();
      },

      error: (error) => {

        console.error(error);

        this.errorMessage =
          'Failed to update address.';
      }

    });
}

  loadAddresses(): void {

    this.loading = true;
    this.errorMessage = '';

    this.addressService.getAll().subscribe({
      next: (data) => {
        this.addresses = data;
        this.loading = false;
      },

      error: (error) => {
        console.error(error);

        this.errorMessage =
          'Failed to load addresses.';

        this.loading = false;
      }
    });
  }

  loadJobTitles(): void {
  this.jobTitleService.getAll().subscribe({
    next: (data) => {
      this.jobTitles = data;
    },
    error: (error) => {
      console.error(error);
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
    }
  });
}

  deleteAddress(id: number): void {

    const confirmed = window.confirm('Are you sure you want to delete this address?');

    if (!confirmed) {
      return;
    }

    this.addressService.delete(id).subscribe({
      next: () => {

        // بدون reload للصفحة
        this.addresses = this.addresses.filter(
          address => address.id !== id
        );

      },

      error: (error) => {
        console.error(error);

        this.errorMessage =
          'Failed to delete address.';
      }
    });
  }


  searchAddresses(): void {

  const search: AddressSearch = {
    searchTerm: this.searchTerm || undefined,
    dateOfBirthFrom: this.dateOfBirthFrom || undefined,
    dateOfBirthTo: this.dateOfBirthTo || undefined
  };

  this.loading = true;
  this.errorMessage = '';

  this.addressService.search(search).subscribe({

    next: (data) => {

      this.addresses = data;
      this.loading = false;

    },

    error: (error) => {

      console.error(error);

      this.errorMessage = 'Failed to search addresses.';
      this.loading = false;

    }

  });
}

clearSearch(): void {

  this.searchTerm = '';
  this.dateOfBirthFrom = '';
  this.dateOfBirthTo = '';

  this.loadAddresses();
}

  exportExcel(): void {

    this.addressService.export().subscribe({
      next: (blob) => {

        const url = window.URL.createObjectURL(blob);

        const anchor = document.createElement('a');

        anchor.href = url;
        anchor.download = 'Addresses.xlsx';

        anchor.click();

        window.URL.revokeObjectURL(url);
      },

      error: (error) => {
        console.error(error);

        this.errorMessage =
          'Failed to export addresses.';
      }
    });
  }

  logout(): void {
    this.authService.logout();

    this.router.navigate(['/login']);
  }


  addNewEntry(): void {
  this.router.navigate(['/address-book/new']);
}

manageJobs(): void {
  this.router.navigate(['/job-titles']);
}

manageDepartments(): void {
  this.router.navigate(['/departments']);
}
}