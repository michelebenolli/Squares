import { RepositoryService } from '../../../services/repository.service';
import { EditorComponent } from '../editor.component';
import { Observable, of, tap } from 'rxjs';

export class EditorBaseContent<T = any> {

  public editor!: EditorComponent;

  getData(service: RepositoryService<T>): Observable<T | undefined> {
    if (!this.editor.config.id) return of(this.editor.config.data);
    this.editor.loading = true;
    return service.getById(this.editor.config.id)
      .pipe(tap(() => this.editor.loading = false));
  }
}
