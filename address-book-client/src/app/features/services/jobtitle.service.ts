import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { JobTitle } from '../../shared/models/ِAddress';

@Injectable({
  providedIn: 'root'
})
export class JobTitleService {

  private apiUrl = 'https://localhost:7107/api/JobTitle';

  constructor(private http: HttpClient) {}

  getAll(): Observable<JobTitle[]> {
    return this.http.get<JobTitle[]>(this.apiUrl);
  }


  getById(id: number): Observable<JobTitle> {
    return this.http.get<JobTitle>(`${this.apiUrl}/${id}`);
  }

  
  create(name: string): Observable<JobTitle> {
    return this.http.post<JobTitle>(
      this.apiUrl,
      { name }
    );
  }

  update(id: number, name: string): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      { name }
    );
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`
    );
  }
}