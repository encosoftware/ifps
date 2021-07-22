import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {
  ApiUsersClient,
  UserDetailsDto,
  ApiCountriesClient,
  CountryListDto,
  ApiLanguagesClient,
  LanguageListDto,
  AddressCreateDto,
  ImageCreateDto,
  LanguageTypeEnum,
  ApiUsersProfileClient,
  UserProfileUpdateDto,
  ApiUsersPasswordClient,
  UserPasswordUpdateDto
} from '../../shared/clients';
import { map } from 'rxjs/operators';
import { IUserBasicInfo, IDropdownViewModel } from 'butor-shared-lib';

@Injectable({
  providedIn: 'root'
})
export class MenuOpenService {

  private openMenuSource = new BehaviorSubject(true);
  currentMenuOpen = this.openMenuSource.asObservable();

  constructor(
    private countryClient: ApiCountriesClient,
    private languageClient: ApiLanguagesClient,
    private profileClient: ApiUsersProfileClient,
    private passwordClient: ApiUsersPasswordClient
  ) { }

  changeMenuOpen(menuOpenState: boolean) {
    this.openMenuSource.next(menuOpenState);
  }

  getUserBasicInfo(id): Observable<IUserBasicInfo> {
    return this.profileClient.getUserProfile(id).pipe(
      map((dto: UserDetailsDto): IUserBasicInfo => ({
        address: dto.address !== undefined ? dto.address.address : null,
        city: dto.address !== undefined ? dto.address.city : null,
        countryId: dto.address !== undefined ? dto.address.countryId : null,
        email: dto.email,
        languageId : dto.language,
        name: dto.name,
        phone: dto.phoneNumber !== undefined ? dto.phoneNumber : null,
        postCode: dto.address !== undefined ? dto.address.postCode : null,
        fileName: dto.image.fileName !== undefined ? dto.image.fileName : null,
        containerName: dto.image.containerName !== undefined ? dto.image.containerName : null
      }))
    );
  }

  updateUserProfile(id: number, userInfo: IUserBasicInfo): Observable<void> {
    const dto = new UserProfileUpdateDto({
      language: LanguageTypeEnum[userInfo.languageId.toString()],
      email: userInfo.email,
      name: userInfo.name,
      phoneNumber: userInfo.phone,
      address: new AddressCreateDto({
        address: userInfo.address,
        city: userInfo.city,
        countryId: userInfo.countryId,
        postCode: userInfo.postCode
      }),
      image: userInfo.fileName ?  new ImageCreateDto({
        containerName: userInfo.containerName,
        fileName: userInfo.fileName
      }) : null
    });
    return this.profileClient.updateUserProfile(id, dto);
  }

  changeUserPassword(id: number, model): Observable<void> {
    const dto = new UserPasswordUpdateDto({
      currentPassword: model.currentPassword,
      newPassword: model.newPassword
    });
    return this.passwordClient.updateUserPassword(id, dto);
  }

  getCountries(): Observable<IDropdownViewModel[]> {
    return this.countryClient.getCountries().pipe(
      map((dto: CountryListDto[]): IDropdownViewModel[] => {
        return dto.map((x: CountryListDto): IDropdownViewModel => ({
          id: x.id,
          name: x.translation
        }));
      })
    );
  }

  getLanguages(): Observable<IDropdownViewModel[]> {
    return this.languageClient.get().pipe(
      map((dto: LanguageListDto[]): IDropdownViewModel[] => {
        return dto.map((x: LanguageListDto): IDropdownViewModel => ({
          id: x.languageType,
          //stringId: x.languageType.toString(),
          name: x.translation
        }));
      })
    );
  }
}
