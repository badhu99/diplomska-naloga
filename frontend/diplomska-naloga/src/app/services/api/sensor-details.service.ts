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

  getSensorData(groupId: string, startDate: null | Date, endDate: null | Date):Observable<ISensorDetails>{
    let url = `${this.endpoint}/GetData/${groupId}`;

    const options: Intl.DateTimeFormatOptions = {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      hour12: false,
  };

    if(startDate !== null){
      url += `?startDate=${startDate.toLocaleString('en-US', options)}`
    }
    if(endDate !== null){
      if(url.includes('?')){
        url += `&`
      }else{
        url += `?`
      }
      url += `endDate=${endDate.toLocaleTimeString('en-US', options)}`
    }
    return this.http.get<ISensorDetails>(url)
  }

  addData(groupId: string, hash: string, data:string):Observable<void>{
    const url = `${this.endpoint}/Add/${groupId}`;
    const bodyRequest = {
      body: data,
      sensorHash: hash
    };
    return this.http.post<void>(url, bodyRequest);
  }
}
