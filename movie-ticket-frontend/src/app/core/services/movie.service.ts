import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Movie } from '../../models/movie/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  private readonly apiUrl = `${environment.apiUrl}/movies`;

  constructor(private http: HttpClient) {}

  // Get all movies
  getMovies(
    pageNumber: number = 1,
    pageSize: number = 20
  ): Observable<Movie[]> {

    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<Movie[]>(
      this.apiUrl,
      { params }
    );
  }

  // Get movie by ID
  getMovieById(id: number): Observable<Movie> {

    return this.http.get<Movie>(
      `${this.apiUrl}/${id}`
    );
  }

  // Search movies by title
  searchMovies(title: string): Observable<Movie[]> {

    const params = new HttpParams()
      .set('title', title);

    return this.http.get<Movie[]>(
      `${this.apiUrl}/search`,
      { params }
    );
  }
}