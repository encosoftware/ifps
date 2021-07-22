export interface WorkStationsPlansListViewModel {
    cuttings?: WorkStationsPlansDetailsModel[] | undefined;
    cncs?: WorkStationsPlansDetailsModel[] | undefined;
    edgebandings?: WorkStationsPlansDetailsModel[] | undefined;
    assemblies?: WorkStationsPlansDetailsModel[] | undefined;
    sortings?: WorkStationsPlansDetailsModel[] | undefined;
    packings?: WorkStationsPlansDetailsModel[] | undefined;
}

export interface WorkStationsPlansDetailsModel  {
    id: number;
    name?: string | undefined;
    endTime?: Date | undefined;
    backgroundColor?: string | undefined;
}