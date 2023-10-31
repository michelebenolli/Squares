import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';

@Injectable()
export class MatPaginatorIt extends MatPaginatorIntl {

  override itemsPerPageLabel = 'Elementi per pagina:';
  override firstPageLabel = 'Prima pagina';
  override lastPageLabel = 'Ultima pagina';
  override nextPageLabel     = 'Pagina successiva';
  override previousPageLabel = 'Pagina precedente';

  override getRangeLabel = (page: number, pageSize: number, length: number) => {
    if (length === 0) {
      return 'Pagina 1 di 1';
    }
    const amountPages = Math.ceil(length / pageSize);
    return `Pagina ${page + 1} di ${amountPages}`;
  };
}
