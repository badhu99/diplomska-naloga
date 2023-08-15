import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/app/environment/environment';
import { Pagination } from 'src/app/interfaces/pagination';
import { ISensorDetails } from 'src/app/interfaces/sensor-details';
import { SensorGroup } from 'src/app/interfaces/sensor-group';

@Injectable({
  providedIn: 'root'
})
export class SensorDetailsService {
  endpoint = `${environment.apiUrl}/api/SensorData`
  constructor(private http:HttpClient) { }

  getSensorData(groupId: string, pageSize: number, pageNumber: number):Observable<ISensorDetails>{
    const url = `${this.endpoint}/GetData/${groupId}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    return this.http.get<ISensorDetails>(url)
  }

  addData(groupId: string, data:string):Observable<void>{
    const url = `${this.endpoint}/Add/${groupId}`;
    const bodyRequest = {
      body: data
    };
    return this.http.post<void>(url, bodyRequest);
  }
}
