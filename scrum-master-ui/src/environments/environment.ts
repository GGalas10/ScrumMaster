import { HttpHeaders } from '@angular/common/http';

export const environment = {
  production: false,
  identityUrl: 'https://localhost:2201',
  sprintUrl: 'https://localhost:2202',
  taskUrl: 'https://localhost:2203',
  projectUrl: 'http://localhost:2205',
  headers: new HttpHeaders({ ScrumMaster: 'true' }),
};
