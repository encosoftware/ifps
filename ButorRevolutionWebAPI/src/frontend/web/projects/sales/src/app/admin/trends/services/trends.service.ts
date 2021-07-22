import { Injectable } from '@angular/core';
import {
    ApiTrendsBoardmaterialsCorpusClient,
    ApiTrendsBoardmaterialsFrontClient,
    ApiTrendsFurnitureunitsClient,
    TrendListItemDtoOfBoardMaterialPreviewDto,
    TrendListItemDtoOfFurnitureUnitPreviewDto
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { ICorpusTrendsListViewModel, ITrendFilterModel, IFurnitureUnitTrendsListViewModel } from '../models/trends.models';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class TrendsService {

    asd: string;

    constructor(
        private corpusTrendsClient: ApiTrendsBoardmaterialsCorpusClient,
        private frontTrendsClient: ApiTrendsBoardmaterialsFrontClient,
        private furnitureUnitTrendsClient: ApiTrendsFurnitureunitsClient,
    ) {
    }

    getCorpusTrends(filter: ITrendFilterModel): Observable<ICorpusTrendsListViewModel[]> {
        const from = new Date(Date.UTC(filter.intervalFrom.getFullYear(), filter.intervalFrom.getMonth(), filter.intervalFrom.getDate()));
        const to = new Date(Date.UTC(filter.intervalTo.getFullYear(), filter.intervalTo.getMonth(), filter.intervalTo.getDate()));
        return this.corpusTrendsClient.getCorpusFurnitureComponentsOrdersTrend(filter.take, from, to).pipe(
            map(res => res.results.map(this.dtoToBoardMaterialViewModel))
        );
    }

    private dtoToBoardMaterialViewModel(dto: TrendListItemDtoOfBoardMaterialPreviewDto): ICorpusTrendsListViewModel {
        return {
            id: dto.orderedItemDto.id,
            ordersCount: dto.ordersCount,
            category: dto.orderedItemDto.category.name,
            code: dto.orderedItemDto.code,
            description: dto.orderedItemDto.description,
            name: dto.orderedItemDto.name,
            hasFiberDirection: dto.orderedItemDto.hasFiberDirection,
            // tslint:disable-next-line: max-line-length
            image: '/api/images?containerName=' + dto.orderedItemDto.image.containerName + '&fileName=' + dto.orderedItemDto.image.fileName
        };
    }

    getFrontTrends(filter: ITrendFilterModel): Observable<any> {
        const from = new Date(Date.UTC(filter.intervalFrom.getFullYear(), filter.intervalFrom.getMonth(), filter.intervalFrom.getDate()));
        const to = new Date(Date.UTC(filter.intervalTo.getFullYear(), filter.intervalTo.getMonth(), filter.intervalTo.getDate()));
        return this.frontTrendsClient.getFrontFurnitureComponentsOrdersTrend(filter.take, from, to).pipe(
            map(res => res.results.map(this.dtoToBoardMaterialViewModel))
        );
    }

    getFurnitureUnitTrends(filter: ITrendFilterModel): Observable<any> {
        const from = new Date(Date.UTC(filter.intervalFrom.getFullYear(), filter.intervalFrom.getMonth(), filter.intervalFrom.getDate()));
        const to = new Date(Date.UTC(filter.intervalTo.getFullYear(), filter.intervalTo.getMonth(), filter.intervalTo.getDate()));
        return this.furnitureUnitTrendsClient.getFurnitureUnitOrdersTrend(filter.take, from, to).pipe(
            map(res => res.results.map(this.dtoToFurnitureUnitViewModel))
        );
    }

    private dtoToFurnitureUnitViewModel(dto: TrendListItemDtoOfFurnitureUnitPreviewDto): IFurnitureUnitTrendsListViewModel {
        return {
            id: dto.orderedItemDto.id,
            ordersCount: dto.ordersCount,
            category: dto.orderedItemDto.category.name,
            code: dto.orderedItemDto.code,
            description: dto.orderedItemDto.description,
            // tslint:disable-next-line: max-line-length
            image: '/api/images?containerName=' + dto.orderedItemDto.imageThumbnail.containerName + '&fileName=' + dto.orderedItemDto.imageThumbnail.fileName
        };
    }

}
