import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslatePipe, TranslateService } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';
import { TokenService } from './Core/Services/token.service';
import { CommonModule } from '@angular/common';
import { DropdownComponent } from './shared/dropdown/dropdown.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    TranslatePipe,
    RouterModule,
    CommonModule,
    DropdownComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'ScrumMaster';
  showMenu = false;
  TransLang: string[] = [];
  constructor(
    private translate: TranslateService,
    private tokenService: TokenService
  ) {
    this.translate.addLangs(['pl', 'en']);
    this.translate.setDefaultLang('pl');
    this.translate.use('pl');
  }
  ngOnInit(): void {
    const savedLang = localStorage.getItem('lang') || 'pl';
    this.translate.use(savedLang);
  }
  setTransLanguage() {
    switch (this.translate.currentLang) {
      case 'pl':
        this.translate.use('en');
        localStorage.setItem('lang', 'en');
        break;
      case 'en':
        this.translate.use('pl');
        localStorage.setItem('lang', 'pl');
        break;
      default:
        this.translate.use('pl');
        localStorage.setItem('lang', 'pl');
        break;
    }
  }
  GetUserName(): string {
    return this.tokenService.GetUserInfos();
  }
  IsLogin() {
    return this.tokenService.IsLogin();
  }
}
