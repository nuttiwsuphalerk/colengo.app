import { AsyncPipe } from '@angular/common';
import { Component, Directive, EventEmitter, Input, Output, QueryList, ViewChildren, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ProductService } from './product-service';
import { Product } from './product-model';
import { NgbdSortableHeader, SortEvent } from './product-directive';
import { FormsModule } from '@angular/forms';
import { NgbHighlight, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'ngbd-table-product',
  templateUrl: './product.html',
  standalone : true,
  imports: [FormsModule, AsyncPipe, NgbHighlight, NgbdSortableHeader, NgbPaginationModule],
  providers: [ProductService],
})

export class NgbdTableComplete
{
  products$!: Observable<Product[]>;
  total$!: Observable<number>;

  @ViewChildren(NgbdSortableHeader) headers!: QueryList<NgbdSortableHeader>;

  constructor(public service: ProductService)
  {
    this.products$ = service.products$;
    this.total$ = service.total$;
  }

  onSort({ column, direction }: SortEvent) {

    // Resetting other headers
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    // Fetching sorted Products from the API
    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
}
