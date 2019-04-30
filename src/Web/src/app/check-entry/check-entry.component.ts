import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-check-entry',
  templateUrl: './check-entry.component.html',
  styles: []
})
export class CheckEntryComponent implements OnInit {

  constructor(private http: HttpClient, private authService: AuthService) { }
  entryCode: string;
  isSuccess = false;
  isReturned = false;
  message: string;
  isError: boolean;

  ngOnInit() {
    
  }

  CanCheckEntry(): boolean {
    if ((this.authService.getClaims().roles as string[]).indexOf('TShirtMeAdminRole') != -1) {
      return true;
    }
    return false;
  }

  async CheckEntry() {
    this.isReturned = false;
    this.isError = false;

    if (this.entryCode != undefined) {
      const headers = new HttpHeaders({ 'Authorization': this.authService.getAuthorizationHeaderValue() });
      await this.http.get("https://kmd-logic-api-prod-tshirtme.azurewebsites.net/api/Entry/" + this.entryCode, { headers })
        .subscribe(response => {
          console.log(response);
          this.isSuccess = response as boolean
          this.isReturned = true;
        }, response => {
          console.log(response);
          this.isError = true;
          if (response.error.message) {
            this.message = response.error.message;
          } else {
            this.message = 'something went terribly wrong';
          }
        });
    }
  }

}
