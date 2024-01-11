import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http'
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/interfaces/pagination';
import { SensorGroup } from 'src/app/interfaces/sensor-group';
import { environment } from 'src/app/environment/environment';

@Injectable({
  providedIn: 'root'
})
export class SensorGroupService {
  endpoint = `${environment.apiUrl}/api/Sensors`
  constructor(private http:HttpClient) { }
  getSensorGroupsPagination(pageNumber: number, pageSize: number, orderDesc: boolean, orderBy: number):Observable<Pagination<SensorGroup>>{
    const url = `${this.endpoint}/GetSensors?pageNumber=${pageNumber}&pageSize=${pageSize}&orderDesc=${orderDesc}&orderBy=${orderBy}`
    return this.http.get<Pagination<SensorGroup>>(url)
  }

  deleteSensorGroup(id:string):Observable<void>{
    const url = `${this.endpoint}/DeleteSensorGroup/${id}`;    
    return this.http.delete<void>(url);
  }

  createNew(name:string, xAxis:string, yAxis:string, description:string, lat:number, long: number):Observable<string>{
    const url = `${this.endpoint}/AddSensorGroup`;
    const body = {
      name,
      columnX:xAxis,
      columnY:yAxis,
      description,
      lat,
      long
    }    
    return this.http.post<string>(url, body);
  }
}
