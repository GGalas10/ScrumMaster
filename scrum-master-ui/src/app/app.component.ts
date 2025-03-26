import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'ScrumMaster';
  TransLang:string[] = [];
  constructor(private translate:TranslateService){
    this.translate.addLangs(['pl','en']);
    this.translate.setDefaultLang('pl');
    this.translate.use('pl');
  }
  ngOnInit(): void {
    this.getTransLanguage();
  }
  getTransLanguage(){
    this.TransLang=[...this.translate.getLangs()];
    console.log(this.TransLang);
    }
  setTransLanguage(){

  }
}
