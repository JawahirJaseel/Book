import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { BookService } from '../../services/book.service';
import { Book } from '../../models/book';

@Component({
  selector: 'app-book-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './book-form.component.html',
  styleUrls: ['./book-form.component.css']
})
export class BookFormComponent implements OnInit {
  book: Book = {
    id: '',
    title: '',
    author: '',
    isbn: '',
    publicationDate: ''
  };
  
  isEditMode: boolean = false;
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(
    private bookService: BookService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.loadBook(id);
    } else {
      // Set default date to today for new books
      this.book.publicationDate = new Date().toISOString().split('T')[0];
    }
  }

  loadBook(id: string): void {
    this.isLoading = true;
    this.bookService.getBook(id).subscribe({
      next: (data) => {
        this.book = data;
        // Format date for the input field (YYYY-MM-DD)
        if (this.book.publicationDate) {
          const dateObj = new Date(this.book.publicationDate);
          this.book.publicationDate = dateObj.toISOString().split('T')[0];
        }
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching book', err);
        this.errorMessage = 'Failed to load book details. Please return to the list.';
        this.isLoading = false;
      }
    });
  }

  onSubmit(): void {
    if (!this.book.title || !this.book.author || !this.book.isbn || !this.book.publicationDate) {
      this.errorMessage = 'Please fill out all fields.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    if (this.isEditMode) {
      this.bookService.updateBook(this.book.id, this.book).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error updating book', err);
          this.errorMessage = 'Failed to update book. Please try again.';
          this.isLoading = false;
        }
      });
    } else {
      // Omit the empty ID for new books to avoid ASP.NET Core Guid parsing errors
      const { id, ...newBookPayload } = this.book;
      
      this.bookService.addBook(newBookPayload as Book).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error adding book', err);
          this.errorMessage = 'Failed to add book. Please try again.';
          this.isLoading = false;
        }
      });
    }
  }
}
