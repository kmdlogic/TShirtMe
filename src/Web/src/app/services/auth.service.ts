import { Injectable } from '@angular/core';
import { UserManager, UserManagerSettings, User } from 'oidc-client';

export function getClientSettings(): UserManagerSettings {
  return {
    authority: 'https://logicidentityprod.b2clogin.com/tfp/logicidentityprod.onmicrosoft.com/B2C_1A_signup_signin/v2.0',
    client_id: '',
    redirect_uri: 'https://tshirtme.kmdlogic.io/auth-callback',
    post_logout_redirect_uri: 'https://tshirtme.kmdlogic.io/',
    response_type: "id_token token",
    scope: "openid https://logicidentityprod.onmicrosoft.com/TShirtMeApi/user_impersonation",
    filterProtocolClaims: true,
    loadUserInfo: false
  };
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor() {
    this.manager.getUser().then(user => {
      this.user = user;
    });
  }
  private user: User = null;
  private manager = new UserManager(getClientSettings());

  isLoggedIn(): boolean {
    return this.user != null && !this.user.expired;
  }
  getClaims(): any {
    return this.user.profile;
  }
  getAuthorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }
  startAuthentication(): Promise<void> {
    return this.manager.signinRedirect();
  }

  completeAuthentication(): Promise<void> {
    return this.manager.signinRedirectCallback().then(user => {
      this.user = user;
    });
  }
}
