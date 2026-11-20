import {HttpParams} from '@angular/common/http';

// Keep the server's full timestamp precision when requesting the next page.
export function pageParams(fromDateTime: Date | string, limit: number, cursorId?: string): HttpParams {
  let params = new HttpParams()
    .set('fromDateTime', fromDateTime instanceof Date ? fromDateTime.toISOString() : fromDateTime)
    .set('limit', limit);
  if (cursorId) params = params.set('cursorId', cursorId);
  return params;
}
