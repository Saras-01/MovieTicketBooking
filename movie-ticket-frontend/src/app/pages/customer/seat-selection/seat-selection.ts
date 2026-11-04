import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { SeatService } from '../../../core/services/seat.service';
import { ShowSeat } from '../../../models/booking/show-seat.model';

@Component({
  selector: 'app-seat-selection',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './seat-selection.html',
  styleUrl: './seat-selection.scss'
})
export class SeatSelection implements OnInit {

  // Selected show ID
  showId = 0;

  // Store seats received from backend
  seats: ShowSeat[] = [];

  // Selected seats
  selectedSeats: ShowSeat[] = [];

  // Loading and error handling
  isLoading = false;
  errorMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private seatService: SeatService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.showId = Number(
      this.route.snapshot.paramMap.get('showId')
    );

    if (!Number.isInteger(this.showId) || this.showId <= 0) {
      this.errorMessage = 'Invalid show ID.';
      return;
    }

    this.loadSeats();
  }

  // Load seats from backend
  loadSeats(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.seatService.getSeatsByShow(this.showId).subscribe({

      next: (response) => {

        console.log('Seats API Response:', response);

        this.seats = response;

        this.isLoading = false;

        this.cdr.detectChanges();

      },

      error: (error) => {

        console.error('Seat loading failed:', error);

        this.errorMessage = 'Unable to load seats. Please try again.';

        this.isLoading = false;

        this.cdr.detectChanges();

      }

    });

  }
  // Check whether a seat is already selected
isSelected(seat: ShowSeat): boolean {
  return this.selectedSeats.some(
    selected => selected.showSeatId === seat.showSeatId
  );
}

// Select or deselect a seat
toggleSeat(seat: ShowSeat): void {

  // Prevent selecting booked seats
  if (seat.status.toLowerCase() !== 'available') {
    return;
  }

  if (this.isSelected(seat)) {

    // Remove seat from selection
    this.selectedSeats = this.selectedSeats.filter(
      selected => selected.showSeatId !== seat.showSeatId
    );

  } else {

    // Add seat to selection
    this.selectedSeats.push(seat);

  }

}

// Calculate total ticket price
getTotalPrice(): number {
  return this.selectedSeats.length * 180;
}

  // Return to previous page
  goBack(): void {
    this.router.navigate(['/customer/dashboard']);
  }

}