import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { PlayerModule } from './player/player.module';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { ActionModule } from './action/action.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MaterialModule,
    PlayerModule,
    ActionModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
