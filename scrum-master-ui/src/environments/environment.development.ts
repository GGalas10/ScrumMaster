import { HttpHeaders } from '@angular/common/http';

export const environment = {
  production: false,
  identityUrl: 'http://localhost:2201',
  sprintUrl: 'http://localhost:2202/Sprint',
  taskUrl: 'http://localhost:2203',
  projectUrl: 'http://localhost:2205',
  headers: new HttpHeaders({ ScrumMaster: 'true' }),
};
