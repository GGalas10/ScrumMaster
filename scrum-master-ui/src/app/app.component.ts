import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {TranslatePipe, TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,TranslatePipe],
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
    }
  setTransLanguage(){
    switch(this.translate.currentLang){
      case "pl" :
        this.translate.use("en"); 
        break;
        case "en" :
          this.translate.use("pl"); 
        break;
        default:
          this.translate.use("pl");
    }
    
  }
}
