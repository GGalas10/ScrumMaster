import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../Services/Auth.service';
import { inject } from '@angular/core';
import { TokenService } from '../Services/token.service';

export const authGuard: CanActivateFn = async (route, state) => {
  const authService = inject(AuthService);
  const tokenService = inject(TokenService);
  const router = inject(Router);
  await authService.Refresh();
  if(tokenService.IsLogin())
    return true;
  else
    return router.parseUrl('/Login');
};
