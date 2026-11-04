import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Show } from '../../models/show/show.model';

@Injectable({
  providedIn: 'root'
})
export class ShowService {

  private readonly apiUrl = `${environment.apiUrl}/shows`;

  constructor(private http: HttpClient) {}

  // Get all shows for a particular movie
  getShowsByMovie(movieId: number): Observable<Show[]> {

    return this.http.get<Show[]>(
      `${this.apiUrl}/movie/${movieId}`
    );

  }
}