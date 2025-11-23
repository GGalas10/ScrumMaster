import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class QueryParameterService {
  constructor(private router: Router) {}

  getQueryParam(name: string): string {
    let route = this.router.routerState.snapshot.root;
    while (route.firstChild) {
      route = route.firstChild;
    }
    const value = route.paramMap.get(name);
    return value ?? '00000000-0000-0000-0000-000000000000';
  }
}
