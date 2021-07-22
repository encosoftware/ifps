import { Injectable } from '@angular/core';
import {
  ApiUsersProfileClient,
  UserProfileUpdateDto,
  LanguageTypeEnum,
  AddressCreateDto,
  ImageCreateDto,
  ApiUsersClient,
  UserDetailsDto,
  ApiCountriesClient
} from '../../shared/clients';
import { Observable } from 'rxjs';
import { IUserBasicInfo } from '../models/account';
import { map } from 'rxjs/operators';
import { CountryListSelectModel } from '../../purchase/models/purchase';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    private profileClient: ApiUsersProfileClient,
    private usersClient: ApiUsersClient,
    private countries: ApiCountriesClient


  ) { }

  updateUserProfile(id: number, userInfo: IUserBasicInfo): Observable<void> {
    const dto = new UserProfileUpdateDto({
      language: userInfo.languageId,
      email: userInfo.email ,
      name: userInfo.name,
      phoneNumber: userInfo.phone,
      address: new AddressCreateDto({
        address: userInfo.address,
        city: userInfo.city,
        countryId: userInfo.countryId,
        postCode: userInfo.postCode
      }),
      image: userInfo.fileName ? new ImageCreateDto({
        containerName: userInfo.containerName,
        fileName: userInfo.fileName
      }) : null
    });
    return this.profileClient.updateUserProfile(dto);
  }


  getUserBasicInfo(id: number): Observable<IUserBasicInfo> {
    return this.usersClient.getById(id).pipe(
      map((dto: UserDetailsDto): IUserBasicInfo => ({
        address: dto.address.address,
        city: dto.address.city,
        countryId: dto.address.countryId,
        email: dto.email,
        languageId: dto.language,
        name: dto.name,
        phone: dto.phoneNumber,
        postCode: dto.address.postCode,
        fileName: dto.image.fileName,
        containerName: dto.image.containerName
      }))
    );
  }

  getCountries(): Observable<CountryListSelectModel[]> {
    return this.countries.getCountries().pipe(
      map((cy) => cy.map((c) =>
        ({
          id: c.id,
          name: c.translation
        })))
    );
  }
}
