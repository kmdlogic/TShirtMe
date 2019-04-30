import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-enter',
  templateUrl: './enter.component.html',
  styles: []
})
export class EnterComponent implements OnInit {


  constructor(private http: HttpClient) { }
  phoneNumber: string;
  code: string;
  isSuccess = false;
  message = '';

  ngOnInit() {
  }

  async submitEntry() {
    this.message = '';
    if (this.phoneNumber != undefined && this.code != undefined) {
      this.http.post('https://kmd-logic-api-prod-tshirtme.azurewebsites.net/api/Entry/',
        {
          phoneNumber: this.phoneNumber,
          code: this.code
        })
        .subscribe(
          (val) => {
            this.phoneNumber = '';
            this.message = 'Thanks, you should recieve a sms containing your entry code shorlty';
          },
          response => {
            console.log(response);
            if (response.error.message) {
              this.message = response.error.message;
            } else {
              this.message = 'something went terribly wrong';
            }
          },
          () => {
            console.log('The POST observable is now completed.');
          });

    } else {
      this.message = 'please enter a phone number and the code displayed at our presentation';
    }
  }

}
