import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ShowSeat } from '../../models/booking/show-seat.model';

@Injectable({
  providedIn: 'root'
})
export class SeatService {

  private readonly apiUrl = `${environment.apiUrl}/shows`;

  constructor(private http: HttpClient) {}

  // Get all seats for a particular show
  getSeatsByShow(showId: number): Observable<ShowSeat[]> {

    return this.http.get<ShowSeat[]>(
      `${this.apiUrl}/${showId}/seats`
    );

  }

}