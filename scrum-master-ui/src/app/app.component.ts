import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {TranslatePipe, TranslateService } from '@ngx-translate/core';
import { TokenService } from './Core/Services/token.service';
import { AuthService } from './Core/Services/Auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,TranslatePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'ScrumMaster';
  TransLang:string[] = [];
  constructor(private translate:TranslateService,private authService:AuthService,private tokenService:TokenService){
    this.translate.addLangs(['pl','en']);
    this.translate.setDefaultLang('pl');
    this.translate.use('pl');
  }
  ngOnInit(): void {
    const savedLang = localStorage.getItem('lang') || 'pl';
    this.translate.use(savedLang);
    this.authService.Refresh();
  }
  setTransLanguage(){
    switch(this.translate.currentLang){
      case "pl" :
        this.translate.use("en"); 
        localStorage.setItem('lang', 'en');
        break;
      case "en" :        
          this.translate.use("pl"); 
          localStorage.setItem('lang', 'pl');
        break;
      default:
          this.translate.use("pl");
          localStorage.setItem('lang', 'pl');
          break;
    }
    
  }
}
