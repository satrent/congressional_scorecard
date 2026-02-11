
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CongressService, CongressMember } from '../services/congress.service';

@Component({
    selector: 'app-state',
    templateUrl: './state.html',
    styleUrl: './state.css',
    standalone: true,
    imports: [RouterLink, CommonModule]
})
export class State implements OnInit {
    stateId: string = '';
    stateName: string = '';
    members: CongressMember[] = [];

    constructor(private route: ActivatedRoute, private congressService: CongressService) { }

    ngOnInit() {
        this.route.paramMap.subscribe((params: ParamMap) => {
            this.stateId = (params.get('id') || '').toUpperCase();
            this.stateName = this.getStateName(this.stateId);
            this.loadMembers();
        });
    }

    loadMembers() {
        if (this.stateId) {
            this.congressService.getMembersByState(this.stateId).subscribe({
                next: (data) => this.members = data,
                error: (err) => console.error('Error fetching members', err)
            });
        }
    }

    getStateName(id: string): string {
        const states: { [key: string]: string } = {
            'CA': 'California',
            'TX': 'Texas',
            'NY': 'New York',
            // Add more as needed
        };
        return states[id] || id;
    }
}
