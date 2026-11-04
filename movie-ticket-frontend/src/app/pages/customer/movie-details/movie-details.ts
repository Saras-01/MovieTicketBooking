import {
  Component,
  OnInit,
  ChangeDetectorRef
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  ActivatedRoute,
  Router,
  RouterModule
} from '@angular/router';

import { MovieService } from '../../../core/services/movie.service';
import { ShowService } from '../../../core/services/show.service';

import { Movie } from '../../../models/movie/movie.model';
import { Show } from '../../../models/show/show.model';


@Component({
  selector: 'app-movie-details',
  standalone: true,

  imports: [
    CommonModule,
    RouterModule
  ],

  templateUrl: './movie-details.html',
  styleUrl: './movie-details.scss'
})


export class MovieDetails implements OnInit {

  // Movie details
  movie: Movie | null = null;

  // Show details
  shows: Show[] = [];

  isShowsLoading = false;
  showsError = '';

  // Loading and error handling
  isLoading = false;
  errorMessage = '';


  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private movieService: MovieService,
    private showService: ShowService,
    private cdr: ChangeDetectorRef
  ) {}


  ngOnInit(): void {
    this.loadMovieDetails();
  }


  // Get selected movie ID from URL
  loadMovieDetails(): void {

    const movieId = Number(
      this.route.snapshot.paramMap.get('id')
    );

    if (!Number.isInteger(movieId) || movieId <= 0) {

      this.errorMessage = 'Invalid movie ID.';

      return;
    }

    this.isLoading = true;

    this.movieService.getMovieById(movieId).subscribe({

      next: (response) => {

        console.log('Movie Details:', response);

        this.movie = response;

        this.isLoading = false;

        // Load available shows for this movie
        this.loadShows(movieId);

        this.cdr.detectChanges();

      },

      error: (error) => {

        console.error(
          'Movie details loading failed:',
          error
        );

        this.errorMessage = 'Unable to load movie details.';

        this.isLoading = false;

        this.cdr.detectChanges();

      }

    });

  }


  // Get available shows for the selected movie
  loadShows(movieId: number): void {

    this.isShowsLoading = true;

    this.showsError = '';

    this.showService.getShowsByMovie(movieId).subscribe({

      next: (response) => {

        console.log('Available Shows:', response);

        this.shows = response;

        this.isShowsLoading = false;

        this.cdr.detectChanges();

      },

      error: (error) => {

        console.error(
          'Show loading failed:',
          error
        );

        this.showsError = 'Unable to load showtimes.';

        this.isShowsLoading = false;

        this.cdr.detectChanges();

      }

    });

  }


  // Return to dashboard
  goBack(): void {

    this.router.navigate(['/customer/dashboard']);

  }
    // ADD THE NEW METHOD HERE
  selectShow(showId: number): void {
    this.router.navigate(['/customer/seats', showId]);
  }


}