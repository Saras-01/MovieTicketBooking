import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { MovieService } from '../../../core/services/movie.service';
import { Movie } from '../../../models/movie/movie.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {

  userName = 'User';

  movies: Movie[] = [];

  searchTerm = '';

  isLoading = false;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private movieService: MovieService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    const user = this.authService.getCurrentUser();

    if (user?.fullName) {
      this.userName = user.fullName;
    }
  }

  ngOnInit(): void {
    this.loadMovies();
  }

  // Load all movies
  loadMovies(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.movieService.getMovies().subscribe({

      next: (response) => {

        console.log('API Response:', response);

        this.movies = response;
        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Movie loading failed:', error);

        this.errorMessage =
          'Unable to load movies. Please try again later.';

        this.isLoading = false;

        this.cdr.detectChanges();
      }
    });
  }

  // Search movies by title
  searchMovies(): void {

    const title = this.searchTerm.trim();

    if (!title) {
      this.loadMovies();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.movieService.searchMovies(title).subscribe({

      next: (response) => {

        console.log('Search Results:', response);

        this.movies = response;
        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Movie search failed:', error);

        this.errorMessage =
          'Unable to search movies. Please try again.';

        this.isLoading = false;

        this.cdr.detectChanges();
      }
    });
  }

  // Select a movie
  selectMovie(movieId: number): void {
    this.router.navigate(['/customer/movies', movieId]);
  }

  // Logout
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

}