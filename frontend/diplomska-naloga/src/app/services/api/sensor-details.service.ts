import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/interfaces/pagination';
import { SensorGroup } from 'src/app/interfaces/sensor-group';

@Injectable({
  providedIn: 'root'
})
export class SensorDetailsService {

  baseUrl = "https://localhost:44388";
  endpoint = "api/SensorData";

  constructor(private http:HttpClient) { }

  getSensorData(groupId: string, pageSize: number, pageNumber: number):Observable<string>{
    const url = `${this.baseUrl}/${this.endpoint}/GetData/${groupId}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    return this.http.get<string>(url)
  }
}
