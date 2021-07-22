import { Injectable } from '@angular/core';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IStoragesListViewModel, IStorageFilterListViewModel, ICountryListModel, INewStorageViewModel } from '../models/stocks.model';
import {
  ApiStoragesClient,
  PagedListDtoOfStorageListDto,
  StorageCreateDto,
  StorageUpdateDto,
  ApiCountriesClient,
  CountryListDto,
  AddressCreateDto
} from '../../../shared/clients';

@Injectable({
  providedIn: 'root'
})
export class StoragesService {

  constructor(
    private storageClient: ApiStoragesClient,
    private countryClient: ApiCountriesClient
  ) { }

  getStorages = (filter: IStorageFilterListViewModel): Observable<PagedData<IStoragesListViewModel>> =>
    this.storageClient.getStorages(
      filter.name,
      filter.address,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfStorageListDto): PagedData<IStoragesListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          name: x.name,
          address: x.address.postCode + ' ' + x.address.city + ', ' + x.address.address
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    )

  getCountries(): Observable<ICountryListModel[]> {
    return this.countryClient.getCountries().pipe(map(x => x.map(this.countryToViewModel)));
  }

  private countryToViewModel(dto: CountryListDto): ICountryListModel {
    let country: ICountryListModel = {
      id: dto.id,
      translation: dto.translation
    };
    return country;
  }

  postStock(model: INewStorageViewModel): Observable<number> {
    let dto = new StorageCreateDto({
      companyId: 1,
      name: model.name,
      address: new AddressCreateDto({
        countryId: model.address.country,
        city: model.address.city,
        postCode: model.address.postCode,
        address: model.address.address,
      })
    });
    return this.storageClient.createStorage(dto);
  }

  putStock(id: number, model: INewStorageViewModel): Observable<void> {
    let dto = new StorageUpdateDto({
      name: model.name,
      address: new AddressCreateDto({
        countryId: model.address.country,
        city: model.address.city,
        postCode: model.address.postCode,
        address: model.address.address,
      })
    });
    return this.storageClient.updateStorage(id, dto);
  }

  deleteStock(id: number): Observable<void> {
    return this.storageClient.deleteStorage(id);
  }

  getStorage(id: number): Observable<INewStorageViewModel> {
    return this.storageClient.getStorage(id).pipe(
      map(res => ({
        name: res.name,
        address: {
          country: res.address.countryId,
          city: res.address.city,
          postCode: res.address.postCode,
          address: res.address.address
        }
      }))
    );
  }

}
