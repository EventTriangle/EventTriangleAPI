import { Injectable } from '@angular/core';
import * as dayjs from "dayjs";

@Injectable({
  providedIn: 'root'
})
export class DateService {
  constructor() { }

  getTimeAndDate(dateString: string) {
    return dayjs(dateString).format("HH:mm DD/MM/YYYY");
  }
}
