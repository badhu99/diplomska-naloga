import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/interfaces/pagination';
import { SensorGroup } from 'src/app/interfaces/sensor-group';

@Injectable({
  providedIn: 'root'
})
export class SensorGroupService {
  baseUrl = "https://localhost:44388";
  endpoint = "api/Sensors";

  constructor(private http:HttpClient) { }

  getSensorGroupsPagination(pageNumber: number, pageSize: number, orderDesc: boolean, orderBy: number):Observable<Pagination<SensorGroup>>{
    const url = `${this.baseUrl}/${this.endpoint}/GetSensors?pageNumber=${pageNumber}&pageSize=${pageSize}&orderDesc=${orderDesc}&orderBy=${orderBy}`
    return this.http.get<Pagination<SensorGroup>>(url)
  }
}
