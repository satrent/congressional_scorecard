
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, RouterLink } from '@angular/router';

@Component({
    selector: 'app-state',
    templateUrl: './state.html',
    styleUrl: './state.css',
    standalone: true,
    imports: [RouterLink]
})
export class State implements OnInit {
    stateId: string = '';
    stateName: string = '';

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        this.route.paramMap.subscribe((params: ParamMap) => {
            this.stateId = params.get('id') || '';
            this.stateName = this.getStateName(this.stateId);
        });
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
