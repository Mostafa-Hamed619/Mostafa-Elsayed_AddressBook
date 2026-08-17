export interface Address {
  id: number;
  fullName: string;

  jobId: number;
  jobTitle: string;

  departmentId: number;
  department: string;

  mobileNumber: string;
  dateOfBirth: string;
  age: number;

  addressLine: string;
  email: string;
  photo?: string;
}

export interface CreateAddress {
  fullName: string;
  jobId: number;
  departmentId: number;
  mobileNumber: string;
  dateOfBirth: string;
  addressLine: string;
  photo?: File;
}

export interface UpdateAddress {
  fullName: string;
  jobId: number;
  departmentId: number;
  mobileNumber: string;
  dateOfBirth: string;
  addressLine: string;
  email: string;
  photo?: File;
}

export interface AddressSearch {
  searchTerm?: string;
  dateOfBirthFrom?: string;
  dateOfBirthTo?: string;
}

export interface JobTitle {
  id: number;
  name: string;
}

export interface Department {
  id: number;
  name: string;
}