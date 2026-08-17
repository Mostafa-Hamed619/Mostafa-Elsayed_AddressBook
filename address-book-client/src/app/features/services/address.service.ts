import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Address, AddressSearch, CreateAddress, UpdateAddress } from '../../shared/models/ِAddress';


@Injectable({
  providedIn: 'root'
})
export class AddressService {

  private readonly apiUrl = 'https://localhost:7107/api/Address';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Address[]> {
    return this.http.get<Address[]>(this.apiUrl);
  }

  getById(id: number): Observable<Address> {
    return this.http.get<Address>(`${this.apiUrl}/${id}`);
  }

  create(formData: FormData): Observable<Address> {
  return this.http.post<Address>(this.apiUrl,formData );
}

 update(id: number, formData: FormData) {
  return this.http.put(`${this.apiUrl}/${id}`,formData);
}

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

 search(filters: AddressSearch): Observable<Address[]> {

  let params = new HttpParams();

  if (filters.searchTerm?.trim()) {
    params = params.set('SearchTerm', filters.searchTerm.trim());
  }

  if (filters.dateOfBirthFrom) {
    params = params.set(
      'DateOfBirthFrom',
      filters.dateOfBirthFrom
    );
  }

  if (filters.dateOfBirthTo) {
    params = params.set(
      'DateOfBirthTo',
      filters.dateOfBirthTo
    );
  }

  return this.http.get<Address[]>(
    `${this.apiUrl}/search`,
    { params }
  );
}


  export(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/export`,
      {
        responseType: 'blob' // means json recieve this as blob not json
      }
    );
  }

  private buildFormData(
    address: CreateAddress | UpdateAddress
  ): FormData {

    const formData = new FormData();

    formData.append('FullName', address.fullName);
    formData.append('JobId', address.jobId.toString());
    formData.append('DepartmentId',address.departmentId.toString());
    formData.append('MobileNumber',address.mobileNumber);
    formData.append('DateOfBirth',address.dateOfBirth);
    formData.append('AddressLine',address.addressLine);

    if ('email' in address && address.email) {
      formData.append('Email', address.email);
    }

    if (address.photo) {
      formData.append('Photo',address.photo,address.photo.name);
    }

    return formData;
  }
}