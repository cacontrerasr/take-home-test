import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { HttpClient } from '@angular/common/http';
import { finalize } from 'rxjs';
import { API_BASE_URL } from './app.config';

interface LoanView {
  id: number;
  amount: number;
  currentBalance: number;
  applicantName: string;
  status: string;
  createdAtUtc: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  private readonly http = inject(HttpClient);
  private readonly apiBaseUrl = inject(API_BASE_URL);

  displayedColumns: string[] = [
    'amount',
    'currentBalance',
    'applicantName',
    'status',
  ];
  loans: LoanView[] = [];
  isLoading = true;
  errorMessage = '';

  get totalOutstanding(): number {
    return this.loans.reduce((sum, loan) => sum + loan.currentBalance, 0);
  }

  get activeLoans(): number {
    return this.loans.filter((loan) => loan.status === 'active').length;
  }

  ngOnInit(): void {
    this.http
      .get<LoanView[]>(`${this.apiBaseUrl}/loans`)
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (loans) => (this.loans = loans),
        error: () =>
          (this.errorMessage =
            'No pudimos cargar los prestamos. Revisa que la API este en ejecucion.'),
      });
  }
}
