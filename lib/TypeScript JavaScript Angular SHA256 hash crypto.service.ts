//depends on npm packages crypto-js and @types/crypto-js
import * as Crypto from 'crypto-js';

import { Injectable } from '@angular/core'

import AppConfig from './_config/app.config'

@Injectable({
  providedIn: 'root'
})
export class CryptoService {

  //yields 64 char hexidecimal notation string. example:
  //6b5f43611d9ef76d8d6cf6364cfe8b32425a8fd6060cf92fe1587e6494168d35
  getClientPasswordHash = (userName : string, clearPassword : string) : string => {
    const hashSource = AppConfig.publicSiteSalt + userName.toLowerCase() + clearPassword
    const clientPasswordHash = Crypto.SHA256(hashSource)
    return clientPasswordHash.toString()
  }

}
