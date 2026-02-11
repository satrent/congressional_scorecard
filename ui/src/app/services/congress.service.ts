
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CongressMember {
    firstName: string;
    lastName: string;
    middleName?: string;
    state: string;
    district?: number;
    advocacyScore: number;
}

@Injectable({
    providedIn: 'root'
})
export class CongressService {
    private apiUrl = 'http://localhost:5065/api/CongressMember';

    constructor(private http: HttpClient) { }

    getMembersByState(stateCode: string): Observable<CongressMember[]> {
        // Add mockdata header if needed for testing, or remove for real data
        // const headers = new HttpHeaders().set('mockdata', 'true');
        return this.http.get<CongressMember[]>(`${this.apiUrl}/state/${stateCode}`);
    }
}
